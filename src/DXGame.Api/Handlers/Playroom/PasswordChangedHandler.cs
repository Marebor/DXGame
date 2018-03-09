using System.Threading.Tasks;
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

        public PasswordChangedHandler(IBroadcaster broadcaster, ILogger<PasswordChangedHandler> logger)
        {
            _broadcaster = broadcaster;
            _logger = logger;
        }

        public async Task HandleAsync(PasswordChanged e) 
            => await _broadcaster.BroadcastAsync<PasswordChanged>(e.RelatedCommand, e.Playroom);
    }
}