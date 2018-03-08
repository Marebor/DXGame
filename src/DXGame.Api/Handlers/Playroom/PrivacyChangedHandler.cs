using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Api.Models.Dto;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Api.Handlers.Playroom
{
    public class PrivacyChangedHandler : IEventHandler<PrivacyChanged>
    {
        IBroadcaster _broadcaster;
        ICache _cache;
        IHandler _handler;

        public PrivacyChangedHandler(IBroadcaster broadcaster, ICache cache, IHandler handler)
        {
            _broadcaster = broadcaster;
            _cache = cache;
            _handler = handler;
        }

        public async Task HandleAsync(PrivacyChanged e) => await _handler
            .LoadAggregate(async () =>
            {
                return await _cache.GetAsync<PlayroomDto>(e.Playroom);
            })
            .Run(playroom => 
            {
                playroom.IsPrivate = e.IsPrivate;
            })
            .OnSuccess(async playroom =>
            {
                await _cache.SetAsync(playroom.Id, playroom);
                await _broadcaster.BroadcastAsync<PrivacyChanged>(e.RelatedCommand, e);
                await _broadcaster.BroadcastAsync<PrivacyChanged>(e.Playroom, e);
            })
            .OnError(async ex => 
            {
                
            })
            .DoNotPropagateException()
            .ExecuteAsync();
    }
}