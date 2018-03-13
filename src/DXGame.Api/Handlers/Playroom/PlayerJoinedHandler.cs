using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayerJoinedHandler : IEventHandler<PlayerJoined>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<PlayerJoinedHandler> _logger;

        public PlayerJoinedHandler(IBroadcaster broadcaster, IHandler handler, ILogger<PlayerJoinedHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }
        public async Task HandleAsync(PlayerJoined e) => await _handler
            .Run(async () => 
            {
                await _broadcaster.BroadcastAsync<PlayerJoined>(e.RelatedCommand, e);
                await _broadcaster.BroadcastAsync<PlayerJoined>(e.Playroom, e);
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