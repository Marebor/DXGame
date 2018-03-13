using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayroomDeletedHandler : IEventHandler<PlayroomDeleted>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<PlayroomDeletedHandler> _logger;

        public PlayroomDeletedHandler(IBroadcaster broadcaster, IHandler handler, ILogger<PlayroomDeletedHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }
        public async Task HandleAsync(PlayroomDeleted e) => await _handler
            .Run(async () => 
            {
                await _broadcaster.BroadcastAsync<PlayroomDeleted>(e.RelatedCommand, e);
                await _broadcaster.BroadcastAsync<PlayroomDeleted>(e);
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