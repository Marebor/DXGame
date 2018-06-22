using System;
using System.Reflection;
using System.Threading.Tasks;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using DXGame.ReadModel.Infrastructure.Abstract;
using DXGame.ReadModel.Models;
using Microsoft.Extensions.Logging;

namespace DXGame.ReadModel.Handlers.Playroom
{
    public class GameStartedHandler : IEventHandler<GameStarted>
    {
        IHandler _handler;
        ILogger<GameStartedHandler> _logger;
        IProjectionService _projectionService;

        public GameStartedHandler(IHandler handler, ILogger<GameStartedHandler> logger, 
            IProjectionService projectionService)
        {
            _handler = handler;
            _logger = logger;
            _projectionService = projectionService;
        }
        public async Task HandleAsync(GameStarted e) => await _handler
            .Run(async () => 
            {
                await _projectionService.UpdateAsync<PlayroomProjection>(e.Playroom, 
                    nameof(PlayroomProjection.ActiveGame), e.Game);
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