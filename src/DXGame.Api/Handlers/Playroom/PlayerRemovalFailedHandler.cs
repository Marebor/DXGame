using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayerRemovalFailedHandler : IEventHandler<PlayerRemovalFailed>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<PlayerRemovalFailedHandler> _logger;

        public PlayerRemovalFailedHandler(IBroadcaster broadcaster, IHandler handler, 
            ILogger<PlayerRemovalFailedHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(PlayerRemovalFailed e) => await _handler
            .Run(async () => 
            {
                await _broadcaster.BroadcastAsync<PlayerRemovalFailed>(e.RelatedCommand, e);
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