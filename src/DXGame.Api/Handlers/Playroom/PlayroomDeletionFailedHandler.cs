using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayroomDeletionFailedHandler : IEventHandler<PlayroomDeletionFailed>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<PlayroomDeletionFailedHandler> _logger;

        public PlayroomDeletionFailedHandler(IBroadcaster broadcaster, IHandler handler, 
            ILogger<PlayroomDeletionFailedHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(PlayroomDeletionFailed e) => await _handler
            .Run(async () => 
            {
                await _broadcaster.BroadcastAsync<PlayroomDeletionFailed>(e.RelatedCommand, e);
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