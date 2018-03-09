using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Api.Models.Dto;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class GameStartFailedHandler : IEventHandler<GameStartFailed>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<GameStartFailedHandler> _logger;

        public GameStartFailedHandler(IBroadcaster broadcaster, IHandler handler, ILogger<GameStartFailedHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(GameStartFailed e) => await _handler
            .Run(async () => 
            {
                await _broadcaster.BroadcastAsync<GameStartFailed>(e.RelatedCommand, e);
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