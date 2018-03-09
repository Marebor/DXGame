using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Api.Models.Dto;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class OwnerChangedHandler : IEventHandler<OwnerChanged>
    {
        IBroadcaster _broadcaster;
        ICache _cache;
        IHandler _handler;
        ILogger<OwnerChangedHandler> _logger;

        public OwnerChangedHandler(IBroadcaster broadcaster, ICache cache, 
            IHandler handler, ILogger<OwnerChangedHandler> logger)
        {
            _broadcaster = broadcaster;
            _cache = cache;
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(OwnerChanged e) => await _handler
            .LoadAggregate(async () =>
            {
                return await _cache.GetAsync<PlayroomDto>(e.Playroom);
            })
            .Run(playroom => 
            {
                playroom.Owner = e.Owner;
            })
            .OnSuccess(async playroom =>
            {
                await _cache.SetAsync(playroom.Id, playroom);
                await _broadcaster.BroadcastAsync<OwnerChanged>(e.RelatedCommand, e);
                await _broadcaster.BroadcastAsync<OwnerChanged>(e.Playroom, e);
            })
            .OnError(ex => 
            {
                _logger.LogError(ex, ex.Message);
                return Task.CompletedTask;
            })
            .DoNotPropagateException()
            .ExecuteAsync();
    }
}