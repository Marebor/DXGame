using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class OwnerChangedHandler : IEventHandler<OwnerChanged>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<OwnerChangedHandler> _logger;

        public OwnerChangedHandler(IBroadcaster broadcaster, IHandler handler, ILogger<OwnerChangedHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(OwnerChanged e) => await _handler
            .Run(async () => 
            {
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