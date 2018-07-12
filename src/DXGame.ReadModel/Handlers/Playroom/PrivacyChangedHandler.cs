using System.Threading.Tasks;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using DXGame.ReadModel.Infrastructure.Abstract;
using DXGame.ReadModel.Models;
using Microsoft.Extensions.Logging;

namespace DXGame.ReadModel.Handlers.Playroom
{
    public class PrivacyChangedHandler : IEventHandler<PrivacyChanged>
    {
        IHandler _handler;
        ILogger _logger;
        IProjectionService _projectionService;

        public PrivacyChangedHandler(IHandler handler, ILogger<PrivacyChangedHandler> logger, 
            IProjectionService projectionService)
        {
            _handler = handler;
            _logger = logger;
            _projectionService = projectionService;
        }
        public async Task HandleAsync(PrivacyChanged e) => await _handler
            .Run(async () => 
            {
                await _projectionService.UpdateAsync<PlayroomProjection>(e.Playroom,
                    nameof(PlayroomProjection.IsPrivate), e.IsPrivate);
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