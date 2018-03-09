using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Api.Models.Dto;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayerLeftHandler : IEventHandler<PlayerLeft>
    {
        IBroadcaster _broadcaster;
        ICache _cache;
        IHandler _handler;
        ILogger<PlayerLeftHandler> _logger;

        public PlayerLeftHandler(IBroadcaster broadcaster, ICache cache, 
            IHandler handler, ILogger<PlayerLeftHandler> logger)
        {
            _broadcaster = broadcaster;
            _cache = cache;
            _handler = handler;
            _logger = logger;
        }
        public async Task HandleAsync(PlayerLeft e) => await _handler
            .LoadAggregate(async () =>
            {
                return await _cache.GetAsync<PlayroomDto>(e.Playroom);
            })
            .Run(playroom => 
            {
                playroom.Players.Remove(e.Player);
            })
            .OnSuccess(async playroom =>
            {
                await _cache.SetAsync(playroom.Id, playroom);
                await _broadcaster.BroadcastAsync<PlayerLeft>(e.RelatedCommand, e);
                await _broadcaster.BroadcastAsync<PlayerLeft>(e.Playroom, e);
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