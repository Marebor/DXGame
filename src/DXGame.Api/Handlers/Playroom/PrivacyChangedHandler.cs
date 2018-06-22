using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class PrivacyChangedHandler : IEventHandler<PrivacyChanged>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<PrivacyChangedHandler> _logger;

        public PrivacyChangedHandler(IBroadcaster broadcaster, IHandler handler, ILogger<PrivacyChangedHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(PrivacyChanged e) => await _handler
            .Run(async () => 
            {
                await _broadcaster.BroadcastAsync<PrivacyChanged>(e.RelatedCommand, e);
                await _broadcaster.BroadcastAsync<PrivacyChanged>(e.Playroom, e);
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