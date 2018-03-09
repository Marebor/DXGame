using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class OwnerChangeFailedHandler : IEventHandler<OwnerChangeFailed>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<OwnerChangeFailedHandler> _logger;

        public OwnerChangeFailedHandler(IBroadcaster broadcaster, IHandler handler, 
            ILogger<OwnerChangeFailedHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(OwnerChangeFailed e) => await _handler
            .Run(async () => 
            {
                await _broadcaster.BroadcastAsync<OwnerChangeFailed>(e.RelatedCommand, e);
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