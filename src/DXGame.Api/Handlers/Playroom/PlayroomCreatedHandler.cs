using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayroomCreatedHandler : IEventHandler<PlayroomCreated>
    {
        IBroadcaster _broadcaster;
        IHandler _handler;
        ILogger<PlayroomCreatedHandler> _logger;

        public PlayroomCreatedHandler(IBroadcaster broadcaster, IHandler handler, ILogger<PlayroomCreatedHandler> logger)
        {
            _broadcaster = broadcaster;
            _handler = handler;
            _logger = logger;
        }
        
        public async Task HandleAsync(PlayroomCreated e) => await _handler
            .Run(async () => 
            {
                e.HidePassword();
                await _broadcaster.BroadcastAsync<PlayroomCreated>(e.RelatedCommand, e);
                await _broadcaster.BroadcastAsync<PlayroomCreated>(e);
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