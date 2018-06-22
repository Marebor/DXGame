using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class GameFinishReceivedHandler : IEventHandler<GameFinishReceived>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<GameFinishReceivedHandler> _logger;

        public GameFinishReceivedHandler(IBroadcaster broadcaster, IHandler handler, ILogger<GameFinishReceivedHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }
        public async Task HandleAsync(GameFinishReceived e) => await _handler
            .Run(async () => 
            {
                await _broadcaster.BroadcastAsync<GameFinishReceived>(e.Playroom, e);
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