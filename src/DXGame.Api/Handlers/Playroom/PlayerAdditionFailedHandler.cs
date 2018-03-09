using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayerAdditionFailedHandler : IEventHandler<PlayerAdditionFailed>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<PlayerAdditionFailedHandler> _logger;

        public PlayerAdditionFailedHandler(IBroadcaster broadcaster, IHandler handler, 
            ILogger<PlayerAdditionFailedHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(PlayerAdditionFailed e) => await _handler
            .Run(async () => 
            {
                await _broadcaster.BroadcastAsync<PlayerAdditionFailed>(e.RelatedCommand, e);
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