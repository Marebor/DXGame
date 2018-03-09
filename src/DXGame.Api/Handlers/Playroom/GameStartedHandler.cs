using System.Reflection;
using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Api.Models;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class GameStartedHandler : IEventHandler<GameStarted>
    {
        IBroadcaster _broadcaster;
        ICache _cache;
        IHandler _handler;
        ILogger<GameStartedHandler> _logger;

        public GameStartedHandler(IBroadcaster broadcaster, ICache cache, 
            IHandler handler, ILogger<GameStartedHandler> logger)
        {
            _broadcaster = broadcaster;
            _cache = cache;
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(GameStarted e) => await _handler
            .LoadAggregate(async () =>
            {
                return await _cache.GetAsync<PlayroomDto>(e.Playroom);
            })
            .Run(playroom => 
            {
                playroom.ActiveGame = e.Game;
            })
            .OnSuccess(async playroom =>
            {
                await _cache.SetAsync(playroom.Id, playroom);
                await _broadcaster.BroadcastAsync<GameStarted>(e.RelatedCommand, e);
                await _broadcaster.BroadcastAsync<GameStarted>(e.Playroom, e);
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