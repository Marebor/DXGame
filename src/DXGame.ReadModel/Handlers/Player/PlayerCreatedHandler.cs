using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Player;
using DXGame.ReadModel.Infrastructure.Abstract;
using DXGame.ReadModel.Models;
using Microsoft.Extensions.Logging;

namespace DXGame.ReadModel.Handlers.Playroom
{
    public class PlayerCreatedHandler : IEventHandler<PlayerCreated>
    {
        IHandler _handler;
        ILogger _logger;
        IMapper _mapper;
        IProjectionService _projectionService;

        public PlayerCreatedHandler(IHandler handler, ILogger<PlayerCreatedHandler> logger, 
            IMapper mapper, IProjectionService projectionService)
        {
            _handler = handler;
            _logger = logger;
            _mapper = mapper;
            _projectionService = projectionService;
        }
        public async Task HandleAsync(PlayerCreated e) => await _handler
            .Run(async () => 
            {
                var projection = _mapper.Map<PlayerProjection>(e);
                await _projectionService.SaveAsync(projection);
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