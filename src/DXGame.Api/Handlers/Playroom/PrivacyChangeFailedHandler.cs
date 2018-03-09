using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class PrivacyChangeFailedHandler : IEventHandler<PrivacyChangeFailed>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<PrivacyChangeFailedHandler> _logger;

        public PrivacyChangeFailedHandler(IBroadcaster broadcaster, IHandler handler, 
            ILogger<PrivacyChangeFailedHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(PrivacyChangeFailed e) => await _handler
            .Run(async () => 
            {
                await _broadcaster.BroadcastAsync<PrivacyChangeFailed>(e.RelatedCommand, e);
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