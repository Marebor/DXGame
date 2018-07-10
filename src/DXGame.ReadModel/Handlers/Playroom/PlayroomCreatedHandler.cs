using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;
using DXGame.ReadModel.Infrastructure.Abstract;
using DXGame.ReadModel.Models;
using Microsoft.Extensions.Logging;

namespace DXGame.ReadModel.Handlers.Playroom
{
    public class PlayroomCreatedHandler : IEventHandler<PlayroomCreated>
    {
        IHandler _handler;
        ILogger<PlayroomCreatedHandler> _logger;
        IMapper _mapper;
        IProjectionService _projectionService;

        public PlayroomCreatedHandler(IHandler handler, ILogger<PlayroomCreatedHandler> logger, 
            IMapper mapper, IProjectionService projectionService)
        {
            _handler = handler;
            _logger = logger;
            _mapper = mapper;
            _projectionService = projectionService;
        }
        public async Task HandleAsync(PlayroomCreated e) => await _handler
            .Run(async () => 
            {
                var playroom = _mapper.Map<PlayroomProjection>(e);
                playroom.Players = new HashSet<Guid>() { playroom.Owner };
                playroom.CompletedGames = new HashSet<Guid>();
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