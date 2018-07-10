using System;
using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Common.Communication;
using DXGame.Messages.Commands.Player;
using DXGame.ReadModel.Infrastructure.Abstract;
using DXGame.ReadModel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Controllers
{
    [Route("[controller]")]
    public class PlayersController : ExtendedController
    {
        IProjectionService _projectionService;

        public PlayersController(IActionResultHelper actionResultHelper, ILogger<PlayersController> logger, 
            IMessageBus messageBus, IProjectionService projectionService) 
            : base(actionResultHelper, logger, messageBus)
        {
            _projectionService = projectionService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
            => await Return(async () => await _projectionService.BrowseAsync<PlayerProjection>());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
            => await Return(async () => await _projectionService.GetAsync<PlayerProjection>(id));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreatePlayer command)
            => await ProcessCommand(command);

        [HttpPut]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePassword command)
            => await ProcessCommand(command);
    }
}