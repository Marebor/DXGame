using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Api.Models.Dto;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayroomCreatedHandler : IEventHandler<PlayroomCreated>
    {
        IBroadcaster _broadcaster;
        ICache _cache;
        IHandler _handler;
        IMapper _mapper;

        public PlayroomCreatedHandler(IBroadcaster broadcaster, ICache cache, IHandler handler, IMapper mapper)
        {
            _broadcaster = broadcaster;
            _cache = cache;
            _handler = handler;
            _mapper = mapper;
        }
        
        public async Task HandleAsync(PlayroomCreated e) => await _handler
            .LoadAggregate(async () =>
            {
                return await Task.FromResult(_mapper.Map<PlayroomDto>(e));
            })
            .Run(playroom => 
            {

            })
            .OnSuccess(async playroom =>
            {
                await _cache.SetAsync(playroom.Id, playroom);
                await _broadcaster.BroadcastAsync<PlayroomCreated>(e.RelatedCommand, e);
                await _broadcaster.BroadcastAsync<PlayroomCreated>(null, e);
            })
            .OnError(async ex => 
            {
                
            })
            .DoNotPropagateException()
            .ExecuteAsync();
    }
}