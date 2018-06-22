using System.Threading.Tasks;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using DXGame.ReadModel.Infrastructure.Abstract;
using DXGame.ReadModel.Models;
using Microsoft.Extensions.Logging;

namespace DXGame.ReadModel.Handlers.Playroom
{
    public class PlayerJoinedHandler : IEventHandler<PlayerJoined>
    {
        IHandler _handler;
        ILogger<PlayerJoinedHandler> _logger;
        IProjectionService _projectionService;

        public PlayerJoinedHandler(IHandler handler, ILogger<PlayerJoinedHandler> logger, 
            IProjectionService projectionService)
        {
            _handler = handler;
            _logger = logger;
            _projectionService = projectionService;
        }
        public async Task HandleAsync(PlayerJoined e) => await _handler
            .LoadAggregate(async () =>
            {
                return await _projectionService.GetAsync<PlayroomProjection>(e.Playroom);
            })
            .Run(playroom => 
            {
                playroom.Players.Add(e.Player);
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