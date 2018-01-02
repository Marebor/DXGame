using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

using DXGame.Models.Entities;
using DXGame.Models.Abstract;

namespace DXGame.Controllers
{
    public class PlayersController : ApiController
    {
        IPlayersRepository _playersRepository;

        public PlayersController(IPlayersRepository playersRepository)
        {
            _playersRepository = playersRepository;
        }

        public IEnumerable<Player> Get()
        {
            return _playersRepository.Players;
        }

        public async Task<IHttpActionResult> Get(string id)
        {
            var player = await _playersRepository.FindAsync(id);

            return player != null ? Ok(player) as IHttpActionResult : NotFound();
        }

        public async Task<IHttpActionResult> Post(string id)
        {
            var player = await _playersRepository.FindAsync(id);

            if (player == null)
            {
                player = await _playersRepository.AddAsync(new Player() { Name = id });
                if (player != null) return Created($"api/players/{id}", player);
                else return InternalServerError();
            }
            else
            {
                return Conflict();
            }
        }

        public async Task<IHttpActionResult> Delete(string id)
        {
            var player = await _playersRepository.FindAsync(id);

            if (player != null)
            {
                player = await _playersRepository.DeleteAsync(id);
                if (player != null) return Ok(player);
                else return InternalServerError();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
