using System.Threading.Tasks;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using DXGame.ReadModel.Infrastructure.Abstract;
using DXGame.ReadModel.Models;
using Microsoft.Extensions.Logging;

namespace DXGame.ReadModel.Handlers.Playroom
{
    public class OwnerChangedHandler : IEventHandler<OwnerChanged>
    {
        IHandler _handler;
        ILogger _logger;
        IProjectionService _projectionService;

        public OwnerChangedHandler(IHandler handler, ILogger<OwnerChangedHandler> logger, 
            IProjectionService projectionService)
        {
            _handler = handler;
            _logger = logger;
            _projectionService = projectionService;
        }
        public async Task HandleAsync(OwnerChanged e) => await _handler
            .Run(async () => 
            {
                await _projectionService.UpdateAsync<PlayroomProjection>(e.Playroom, 
                    nameof(PlayroomProjection.Owner), e.Owner);
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