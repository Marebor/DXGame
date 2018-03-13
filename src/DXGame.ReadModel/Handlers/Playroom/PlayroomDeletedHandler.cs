using System.Threading.Tasks;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using DXGame.ReadModel.Infrastructure.Abstract;
using DXGame.ReadModel.Models;
using Microsoft.Extensions.Logging;

namespace DXGame.ReadModel.Handlers.Playroom
{
    public class PlayroomDeletedHandler : IEventHandler<PlayroomDeleted>
    {
        IHandler _handler;
        ILogger<PlayroomDeletedHandler> _logger;
        IProjectionService _projectionService;

        public PlayroomDeletedHandler(IHandler handler, ILogger<PlayroomDeletedHandler> logger, 
            IProjectionService projectionService)
        {
            _handler = handler;
            _logger = logger;
            _projectionService = projectionService;
        }
        public async Task HandleAsync(PlayroomDeleted e) => await _handler
            .Run(async () => 
            {
                await _projectionService.DeleteAsync<PlayroomProjection>(e.Playroom);
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