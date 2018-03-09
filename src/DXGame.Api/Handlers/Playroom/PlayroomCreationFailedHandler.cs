using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayroomCreationFailedHandler : IEventHandler<PlayroomCreationFailed>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<PlayroomCreationFailedHandler> _logger;

        public PlayroomCreationFailedHandler(IBroadcaster broadcaster, IHandler handler, 
            ILogger<PlayroomCreationFailedHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(PlayroomCreationFailed e) => await _handler
            .Run(async () => 
            {
                await _broadcaster.BroadcastAsync<PlayroomCreationFailed>(e.RelatedCommand, e);
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