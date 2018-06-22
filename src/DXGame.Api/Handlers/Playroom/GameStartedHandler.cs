using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class GameStartedHandler : IEventHandler<GameStarted>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<GameStartedHandler> _logger;

        public GameStartedHandler(IBroadcaster broadcaster, IHandler handler, ILogger<GameStartedHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(GameStarted e) => await _handler
            .Run(async () => 
            {
                await _broadcaster.BroadcastAsync<GameStarted>(e.RelatedCommand, e);
                await _broadcaster.BroadcastAsync<GameStarted>(e.Playroom, e);
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