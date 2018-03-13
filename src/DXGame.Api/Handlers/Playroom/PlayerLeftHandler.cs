using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayerLeftHandler : IEventHandler<PlayerLeft>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<PlayerLeftHandler> _logger;

        public PlayerLeftHandler(IBroadcaster broadcaster, IHandler handler, ILogger<PlayerLeftHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }
        public async Task HandleAsync(PlayerLeft e) => await _handler
            .Run(async () => 
            {
                await _broadcaster.BroadcastAsync<PlayerLeft>(e.RelatedCommand, e);
                await _broadcaster.BroadcastAsync<PlayerLeft>(e.Playroom, e);
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