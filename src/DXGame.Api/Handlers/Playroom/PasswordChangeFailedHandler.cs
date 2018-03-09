using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Api.Models;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class PasswordChangeFailedHandler : IEventHandler<PasswordChangeFailed>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<PasswordChangeFailedHandler> _logger;

        public PasswordChangeFailedHandler(IBroadcaster broadcaster, IHandler handler, 
            ILogger<PasswordChangeFailedHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(PasswordChangeFailed e) => await _handler
            .Run(async () => 
            {
                await _broadcaster.BroadcastAsync<PasswordChangeFailed>(e.RelatedCommand, e);
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