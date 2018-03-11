using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Api.Models;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayroomCreatedHandler : IEventHandler<PlayroomCreated>
    {
        IBroadcaster _broadcaster;
        ICache _cache;
        IHandler _handler;
        ILogger<PlayroomCreatedHandler> _logger;
        IMapper _mapper;

        public PlayroomCreatedHandler(IBroadcaster broadcaster, ICache cache, 
            IHandler handler, IMapper mapper, ILogger<PlayroomCreatedHandler> logger)
        {
            _broadcaster = broadcaster;
            _cache = cache;
            _handler = handler;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task HandleAsync(PlayroomCreated e) => await _handler
            .Run(async () => 
            {
                var playroom = _mapper.Map<PlayroomDto>(e);
                await _cache.SetAsync(playroom.Id, playroom);
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