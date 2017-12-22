using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Threading.Tasks;

using DXGame.Models.Entities;
using DXGame.Models.Abstract;
using DXGame.Models.Events;
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

        public IEnumerable<Playroom> Get()
        {
            return _playroomsRepository.Playrooms;
        }

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
                playroom = new Playroom() { Name = id, Players = new List<Player>() };
                var dxEvent = CreateDXEvent(new PlayroomCreatedEvent() { PlayroomName = playroom.Name, PerformedBy = player.Name, DatePerformed = DateTime.Now });
                dxEvent = await _eventsRepository.AddAsync(dxEvent);
                if (dxEvent != null)
                {
                    _broadcast.Broadcast(dxEvent.Content);
                    return Created($"api/playrooms/{id}", playroom);
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
                var dxEvent = CreateDXEvent(new PlayroomDeletedEvent() { PlayroomName = playroom.Name, PerformedBy = player.Name, DatePerformed = DateTime.Now });
                dxEvent = await _eventsRepository.AddAsync(dxEvent);
                if (dxEvent != null)
                {
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
                    var dxEvent = CreateDXEvent(new PlayerJoinedPlayroomEvent() { PerformedBy = player.Name, DatePerformed = DateTime.Now, PlayroomName = playroom.Name });
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
                    var dxEvent = CreateDXEvent(new PlayerLeftPlayroomEvent() { PerformedBy = player.Name, DatePerformed = DateTime.Now, PlayroomName = playroom.Name });
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
        public IEnumerable<string> GetEvents(string id)
        {
            return _eventsRepository.Events.Where(e => e.PlayroomName == id).OrderBy(e => e.DatePerformed).Select(e => e.Content);
        }

        [Route("api/playrooms/{id}/events")]
        [HttpPost]
        public async Task<IHttpActionResult> PostEvent(string id)
        {
            (var errorResponse, var player, var playroom) = await CheckRequestValidityAsync(id, true);
            if (errorResponse != null) return errorResponse;

            var message = await Request.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(message) && playroom.Players.FirstOrDefault(p => p.Name == player.Name) != null)
            {
                var dxEvent = new DXEvent() { PerformedBy = player.Name, DatePerformed = DateTime.Now, PlayroomName = playroom.Name, Content = message };
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
                result = BadRequest(_requestPlayernameProvider.CannotRetrievePlayernameErrorMessage);
                return (result, player, playroom);
            }

            player = await _playersRepository.FindAsync(playername);
            if (player == null)
            {
                result = BadRequest($"Player with name {playername} doesn't exist");
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

        private DXEvent CreateDXEvent(EventContentBase content)
        {
            var dxEvent = new DXEvent()
            {
                PerformedBy = content.PerformedBy,
                DatePerformed = content.DatePerformed,
                PlayroomName = content.PlayroomName,
                Content = JsonConvert.SerializeObject(content)
            };

            return dxEvent;
        }
    }
}
