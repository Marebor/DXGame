using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

using System.Threading.Tasks;

using DXGame.Models.Entities;
using DXGame.Models.Abstract;
using DXGame.Providers.Abstract;

using Newtonsoft.Json;

namespace DXGame.Controllers
{
    public class PlayroomsController : ApiController
    {
        private readonly IPlayroomsRepository _playroomsRepository;
        private readonly IPlayersRepository _playersRepository;
        private readonly IEventsRepository _eventsRepository;
        private readonly IBroadcast _broadcast;
        private readonly IRequestPlayernameProvider _requestPlayernameProvider;

        public PlayroomsController(
                IPlayroomsRepository playroomsRepository, 
                IPlayersRepository playersRepository,
                IEventsRepository eventsRepository,
                IBroadcast broadcast,
                IRequestPlayernameProvider requestPlayernameProvider
            )
        {
            _playroomsRepository = playroomsRepository;
            _playersRepository = playersRepository;
            _eventsRepository = eventsRepository;
            _broadcast = broadcast;
            _requestPlayernameProvider = requestPlayernameProvider;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IEnumerable<Playroom> Get()
        {
            return _playroomsRepository.Playrooms;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var playroom = await _playroomsRepository.FindAsync(id);
            return playroom != null ? Ok(playroom) as IHttpActionResult : NotFound();
        }

        public async Task<IHttpActionResult> Post(string id)
        {
            (var errorResponse, var player, var playroom) = await CheckRequestValidityAsync(id, false);
            if (errorResponse != null) return errorResponse;

            if (playroom == null)
            {
                playroom = await _playroomsRepository.AddAsync(new Playroom() { Name = id, Players = new List<Player>() });
                if (playroom != null)
                {
                    var eventContent = new
                    {
                        Description = "PlayroomCreated",
                        PerformedBy = player.Name,
                        DatePerformed = DateTime.Now,
                        PlayroomName = playroom.Name,
                    };
                    var dxEvent = new DXEvent(eventContent)
                        { PlayroomName = eventContent.PlayroomName, PerformedBy = eventContent.PerformedBy, DatePerformed = eventContent.DatePerformed };
                    dxEvent = await _eventsRepository.AddAsync(dxEvent);
                    _broadcast.Broadcast(dxEvent.Content);
                    return Created($"api/playrooms/{id}".ToLower(), playroom);
                }
                else
                {
                    await _playersRepository.DeleteAsync(playroom.Name);
                    return InternalServerError();
                }
            }
            else
            {
                return Conflict();
            }
        }

        public async Task<IHttpActionResult> Delete(string id)
        {
            (var errorResponse, var player, var playroom) = await CheckRequestValidityAsync(id, true);
            if (errorResponse != null) return errorResponse;

            if (playroom != null)
            {
                playroom = await _playroomsRepository.DeleteAsync(playroom.Name);
                if (playroom != null)
                {
                    var eventContent = new
                    {
                        Description = "PlayroomDeleted",
                        PerformedBy = player.Name,
                        DatePerformed = DateTime.Now,
                        PlayroomName = playroom.Name,
                    };
                    var dxEvent = new DXEvent(eventContent)
                        { PlayroomName = eventContent.PlayroomName, PerformedBy = eventContent.PerformedBy, DatePerformed = eventContent.DatePerformed };
                    dxEvent = await _eventsRepository.AddAsync(dxEvent);
                    _broadcast.Broadcast(dxEvent.Content);
                    return Ok(playroom);
                }
                else
                {
                    await _playroomsRepository.AddAsync(playroom);
                    return InternalServerError();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [Route("api/playrooms/{id}/join")]
        [HttpPut]
        public async Task<IHttpActionResult> Join(string id)
        {
            (var errorResponse, var player, var playroom) = await CheckRequestValidityAsync(id, true);
            if (errorResponse != null) return errorResponse;

            if (playroom.Players.FirstOrDefault(p => p.Name == player.Name) == null)
            {
                playroom = await _playroomsRepository.AddPlayerToPlayroomAsync(player, playroom.Name);
                if (playroom != null)
                {
                    var eventContent = new
                    {
                        Description = "PlayerJoinedPlayroom",
                        PerformedBy = player.Name,
                        DatePerformed = DateTime.Now,
                        PlayroomName = playroom.Name,
                    };
                    var dxEvent = new DXEvent(eventContent)
                        { PlayroomName = eventContent.PlayroomName, PerformedBy = eventContent.PerformedBy, DatePerformed = eventContent.DatePerformed };
                    dxEvent = await _eventsRepository.AddAsync(dxEvent);
                    if (dxEvent != null)
                    {
                        _broadcast.Broadcast(dxEvent.Content);
                        return Ok();
                    }
                    else
                    {
                        await _playroomsRepository.RemovePlayerFromPlayroomAsync(player.Name, playroom.Name);
                        return InternalServerError();
                    }                    
                }
                else
                {
                    return InternalServerError();
                }
            }
            else
            {
                return Conflict();
            }
        }

        [Route("api/playrooms/{id}/leave")]
        [HttpPut]
        public async Task<IHttpActionResult> Leave(string id)
        {
            (var errorResponse, var player, var playroom) = await CheckRequestValidityAsync(id, true);
            if (errorResponse != null) return errorResponse;

            if (playroom.Players.FirstOrDefault(p => p.Name == player.Name) != null)
            {
                playroom = await _playroomsRepository.RemovePlayerFromPlayroomAsync(player.Name, playroom.Name);
                if (playroom != null)
                {
                    var eventContent = new
                    {
                        Description = "PlayerLeftPlayroom",
                        PerformedBy = player.Name,
                        DatePerformed = DateTime.Now,
                        PlayroomName = playroom.Name,
                    };
                    var dxEvent = new DXEvent(eventContent)
                        { PlayroomName = eventContent.PlayroomName, PerformedBy = eventContent.PerformedBy, DatePerformed = eventContent.DatePerformed };
                    dxEvent = await _eventsRepository.AddAsync(dxEvent);
                    if (dxEvent != null)
                    {
                        _broadcast.Broadcast(dxEvent.Content);
                        return Ok();
                    }
                    else
                    {
                        await _playroomsRepository.AddPlayerToPlayroomAsync(player, playroom.Name);
                        return InternalServerError();
                    }
                }
                else
                {
                    return InternalServerError();
                }
            }
            else
            {
                return BadRequest($"Playroom {playroom.Name} does not contain player {player.Name}");
            }
        }

        [Route("api/playrooms/{id}/events")]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> GetEvents(string id)
        {
            var playroom = await _playroomsRepository.FindAsync(id);
            if (playroom == null) return NotFound();

            var events = _eventsRepository.Events.Where(e => e.PlayroomName == id)?.OrderBy(e => e.DatePerformed)?.Select(e => e.Content);

            return Ok(events);
        }

        [Route("api/playrooms/{id}/events")]
        [HttpPost]
        public async Task<IHttpActionResult> PostEvent(string id, [FromBody] string eventContent)
        {
            (var errorResponse, var player, var playroom) = await CheckRequestValidityAsync(id, true);
            if (errorResponse != null) return errorResponse;
            if (!DXEvent.ValidateContent(eventContent)) return BadRequest("Posted event is not an object");

            if (!string.IsNullOrEmpty(eventContent) && playroom.Players.FirstOrDefault(p => p.Name == player.Name) != null)
            {
                var dxEvent = new DXEvent(eventContent) { PerformedBy = player.Name, DatePerformed = DateTime.Now, PlayroomName = playroom.Name };
                dxEvent = await _eventsRepository.AddAsync(dxEvent);
                if (dxEvent != null)
                {
                    _broadcast.BroadcastGroup(dxEvent.Content, dxEvent.PlayroomName);
                    return Ok();
                }
                else
                {
                    return InternalServerError();
                }                
            }
            else
            {
                return BadRequest();
            }
        } 

        private async Task<(IHttpActionResult errorResult, Player player, Playroom playroom)> CheckRequestValidityAsync(string playroomName, bool playroomShouldExist)
        {
            IHttpActionResult result = null;
            Player player = null;
            Playroom playroom = null;

            var playername = _requestPlayernameProvider.GetPlayername();
            if (string.IsNullOrEmpty(playername) || string.IsNullOrWhiteSpace(playername))
            {
                result = BadRequest(_requestPlayernameProvider.CannotRetrievePlayernameErrorMessage ?? string.Empty);
                return (result, player, playroom);
            }

            player = await _playersRepository.FindAsync(playername);
            if (player == null)
            {
                result = BadRequest($"Player with name {playername ?? string.Empty} doesn't exist");
                return (result, player, playroom);
            }

            playroom = await _playroomsRepository.FindAsync(playroomName);
            if (playroom == null && playroomShouldExist)
            {
                result = NotFound();
                return (result, player, playroom);
            }                

            return (result, player, playroom);
        }
    }
}
