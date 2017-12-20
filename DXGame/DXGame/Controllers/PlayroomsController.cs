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
        private readonly IRequestPlayroomNameProvider _requestPlayroomNameProvider;

        public PlayroomsController(
            IPlayroomsRepository playroomsRepository, 
            IPlayersRepository playersRepository,
            IEventsRepository eventsRepository,
            IBroadcast broadcast,
            IRequestPlayernameProvider requestPlayernameProvider,
            IRequestPlayroomNameProvider requestPlayroomNameProvider
            )
        {
            _playroomsRepository = playroomsRepository;
            _playersRepository = playersRepository;
            _eventsRepository = eventsRepository;
            _broadcast = broadcast;
            _requestPlayernameProvider = requestPlayernameProvider;
            _requestPlayroomNameProvider = requestPlayroomNameProvider;
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
            var playername = _requestPlayernameProvider.GetPlayername();
            if (string.IsNullOrEmpty(playername) || string.IsNullOrWhiteSpace(playername)) return BadRequest("Wrong playername. Only existing player can create a new playroom.");
            var player = await _playersRepository.FindAsync(playername);
            if (player == null) return BadRequest($"Player with name {playername} doesn't exist. Only existing player can create a new playroom.");
            var playroom = await _playroomsRepository.AddAsync(new Playroom() { Name = id });
            if (playroom != null)
            {
                var dxEvent = CreateDXEvent(new PlayroomCreatedEvent() { PlayroomName = playroom.Name, PerformedBy = player.Name, DatePerformed = DateTime.Now });
                dxEvent = await _eventsRepository.AddAsync(dxEvent);
                if (dxEvent != null)
                {
                    _broadcast.Broadcast(dxEvent.Content);
                    return Created(Url.Link("DefaultApi", new { controller = ControllerContext.ControllerDescriptor.ControllerName, id = id }), playroom);
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
            var playername = _requestPlayernameProvider.GetPlayername();
            if (string.IsNullOrEmpty(playername) || string.IsNullOrWhiteSpace(playername)) return BadRequest("Wrong playername. Only existing player can delete playroom.");
            var player = await _playersRepository.FindAsync(playername);
            if (player == null) return BadRequest($"Player with name {playername} doesn't exist. Only existing player can create a new playroom.");
            var playroom = await _playroomsRepository.DeleteAsync(id);
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
            var playername = _requestPlayernameProvider.GetPlayername();
            if (string.IsNullOrEmpty(playername) || string.IsNullOrWhiteSpace(playername)) return BadRequest("Wrong playername. Name has to contain even one non-whitespace sign");
            var playroom = await _playroomsRepository.FindAsync(id);
            if (playroom == null) return NotFound();
            var player = await _playersRepository.FindAsync(playername);
            if (player == null) return BadRequest($"Player with name {playername} doesn't exist");

            if (playroom.Players.FirstOrDefault(p => p.Name == playername) == null)
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
            var playername = _requestPlayernameProvider.GetPlayername();
            if (string.IsNullOrEmpty(playername) || string.IsNullOrWhiteSpace(playername)) return BadRequest("Wrong playername");
            var playroom = await _playroomsRepository.FindAsync(id);
            if (playroom == null) return NotFound();
            var player = await _playersRepository.FindAsync(playername);
            if (player == null) return BadRequest($"Player with name {playername} doesn't exist");

            if (playroom.Players.FirstOrDefault(p => p.Name == playername) != null)
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
                return BadRequest($"Playroom {playroom.Name} does not contain player {playername}");
            }
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
