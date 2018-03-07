using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Api.Models.Dto;
using DXGame.Common.Communication;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayroomCreatedHandler : IEventHandler<PlayroomCreated>
    {
        IBroadcaster _broadcaster;
        ICache _cache;
        IMapper _mapper;

        public PlayroomCreatedHandler(IBroadcaster broadcaster, ICache cache, IMapper mapper)
        {
            _broadcaster = broadcaster;
            _cache = cache;
            _mapper = mapper;
        }
        
        public async Task HandleAsync(PlayroomCreated e)
        {
            var playroom = _mapper.Map<PlayroomDto>(e);
            _cache.Set(playroom.Id, playroom);
            await _broadcaster.BroadcastAsync<PlayroomCreated>(playroom);
        }
    }
}