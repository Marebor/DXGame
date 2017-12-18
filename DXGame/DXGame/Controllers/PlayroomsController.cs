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
    public class PlayroomsController : ApiController
    {
        private readonly IPlayroomsRepository _playroomsRepository;
        private readonly IBroadcastCommon _broadcastCommon;

        public PlayroomsController(IPlayroomsRepository playroomsRepository, IBroadcastCommon broadcastCommon)
        {
            _playroomsRepository = playroomsRepository;
            _broadcastCommon = broadcastCommon;
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
    }
}
