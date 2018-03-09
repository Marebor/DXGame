using System;
using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Api.Models.Dto;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class GameFinishReceivedHandler : IEventHandler<GameFinishReceived>
    {
        IBroadcaster _broadcaster;
        ICache _cache;
        IHandler _handler;
        ILogger<GameFinishReceivedHandler> _logger;

        public GameFinishReceivedHandler(IBroadcaster broadcaster, ICache cache, 
            IHandler handler, ILogger<GameFinishReceivedHandler> logger)
        {
            _broadcaster = broadcaster;
            _cache = cache;
            _handler = handler;
            _logger = logger;
        }
        public async Task HandleAsync(GameFinishReceived e) => await _handler
            .LoadAggregate(async () =>
            {
                return await _cache.GetAsync<PlayroomDto>(e.Playroom);
            })
            .Run(playroom => 
            {
                playroom.CompletedGames.Add((Guid)playroom.ActiveGame);
                playroom.ActiveGame = null;
            })
            .OnSuccess(async playroom =>
            {
                await _cache.SetAsync(playroom.Id, playroom);
                await _broadcaster.BroadcastAsync<GameFinishReceived>(e.Playroom, e);
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