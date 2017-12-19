using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Threading.Tasks;

using DXGame.Models.Entities;
using DXGame.Models.Abstract;
using DXGame.Providers;

namespace DXGame.Controllers
{
    public class PlayroomsController : ApiController
    {
        private readonly IPlayroomsRepository _playroomsRepository;
        private readonly IPlayersRepository _playersRepository;
        private readonly IBroadcastCommon _broadcastCommon;
        private readonly IRequestPlayernameProvider _requestPlayernameProvider;

        public PlayroomsController(
            IPlayroomsRepository playroomsRepository, 
            IPlayersRepository playersRepository, 
            IBroadcastCommon broadcastCommon, 
            IRequestPlayernameProvider requestPlayernameProvider
            )
        {
            _playroomsRepository = playroomsRepository;
            _playersRepository = playersRepository;
            _broadcastCommon = broadcastCommon;
            _requestPlayernameProvider = requestPlayernameProvider;
        }

        public IEnumerable<Playroom> Get()
        {
            return _playroomsRepository.Playrooms;
        }

        public async Task<IHttpActionResult> Post(string name)
        {
            var playroom = await _playroomsRepository.AddAsync(name);
            if (playroom != null) _broadcastCommon.PlayroomCreated(name);

            return playroom != null ? 
                Created(Url.Link("DefaultApi", new { controller = ControllerContext.ControllerDescriptor.ControllerName, id = name }), playroom) as IHttpActionResult : 
                Conflict();
        }

        public async Task<IHttpActionResult> Delete(string name)
        {
            var playroom = await _playroomsRepository.DeleteAsync(name);
            if (playroom != null) _broadcastCommon.PlayroomDeleted(name);

            return playroom != null ? Ok(playroom) as IHttpActionResult : NotFound();
        }

        [Route("api/playrooms/{name}/join")]
        [HttpPut]
        public async Task<IHttpActionResult> Join(string name)
        {
            var playername = _requestPlayernameProvider.GetPlayername();
            if (string.IsNullOrEmpty(playername) || string.IsNullOrWhiteSpace(playername)) return BadRequest("Wrong playername");
            var playroom = await _playroomsRepository.FindAsync(name);
            if (playroom == null) return NotFound();
            var player = await _playersRepository.FindAsync(playername);
            if (player == null) return BadRequest($"Player with name {playername} doesn't exist");

            if (playroom.Players.FirstOrDefault(p => p.Name == playername) == null)
            {
                playroom.Players.Add(player);
                playroom = await _playroomsRepository.UpdateAsync(playroom);
                if (playroom != null)
                {
                    _broadcastCommon.PlayerJoined(playername, name);
                    return Ok();
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

        [Route("api/playrooms/{name}/leave")]
        [HttpPut]
        public async Task<IHttpActionResult> Leave(string name)
        {
            var playername = _requestPlayernameProvider.GetPlayername();
            if (string.IsNullOrEmpty(playername) || string.IsNullOrWhiteSpace(playername)) return BadRequest("Wrong playername");
            var playroom = await _playroomsRepository.FindAsync(name);
            if (playroom == null) return NotFound();
            var player = await _playersRepository.FindAsync(playername);
            if (player == null) return BadRequest($"Player with name {playername} doesn't exist");

            if (playroom.Players.FirstOrDefault(p => p.Name == playername) != null)
            {
                playroom.Players.Remove(player);
                playroom = await _playroomsRepository.UpdateAsync(playroom);
                if (playroom != null)
                {
                    _broadcastCommon.PlayerLeft(playername, name);
                    return Ok();
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
    }
}
