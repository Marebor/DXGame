using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Api.Models;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class PasswordChangedHandler : IEventHandler<PasswordChanged>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<PasswordChangedHandler> _logger;

        public PasswordChangedHandler(IBroadcaster broadcaster, IHandler handler, ILogger<PasswordChangedHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(PasswordChanged e) => await _handler
            .Run(async () => 
            {
                await _broadcaster.BroadcastAsync<PasswordChanged>(e.RelatedCommand, e.Playroom);
                await _broadcaster.BroadcastAsync<PasswordChanged>(e.Playroom, e.Playroom);
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