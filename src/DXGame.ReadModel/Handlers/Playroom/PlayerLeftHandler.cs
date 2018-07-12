using System.Threading.Tasks;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using DXGame.ReadModel.Infrastructure.Abstract;
using DXGame.ReadModel.Models;
using Microsoft.Extensions.Logging;

namespace DXGame.ReadModel.Handlers.Playroom
{
    public class PlayerLeftHandler : IEventHandler<PlayerLeft>
    {
        IHandler _handler;
        ILogger _logger;
        IProjectionService _projectionService;

        public PlayerLeftHandler(IHandler handler, ILogger<PlayerLeftHandler> logger, 
            IProjectionService projectionService)
        {
            _handler = handler;
            _logger = logger;
            _projectionService = projectionService;
        }
        public async Task HandleAsync(PlayerLeft e) => await _handler
            .LoadAggregate(async () =>
            {
                return await _projectionService.GetAsync<PlayroomProjection>(e.Playroom);
            })
            .Run(playroom => 
            {
                playroom.Players.Remove(e.Player);
            })
            .OnSuccess(async playroom =>
            {
                await _projectionService.SaveAsync(playroom);
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