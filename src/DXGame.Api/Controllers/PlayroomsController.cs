using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Common.Communication;
using DXGame.Messages.Commands.Playroom;
using DXGame.ReadModel.Infrastructure.Abstract;
using DXGame.ReadModel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Controllers
{
    [Route("[controller]")]
    public class PlayroomsController : ExtendedController
    {
        IProjectionService _projectionService;

        public PlayroomsController(IActionResultHelper actionResultHelper, ILogger<PlayroomsController> logger, 
            IMessageBus messageBus, IProjectionService projectionService) 
            : base(actionResultHelper, logger, messageBus)
        {
            _projectionService = projectionService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
            => await Return(async () => await _projectionService.BrowseAsync<PlayroomProjection>());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
            => await Return(async () => await _projectionService.GetAsync<PlayroomProjection>(id));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreatePlayroom command)
            => await ProcessCommand(command);

        [HttpPut("delete")]
        public async Task<IActionResult> Delete([FromBody]DeletePlayroom command)
            => await ProcessCommand(command);

        [HttpPut("addplayer")]
        public async Task<IActionResult> AddPlayer([FromBody]AddPlayer command)
            => await ProcessCommand(command);

        [HttpPut("removeplayer")]
        public async Task<IActionResult> RemovePlayer([FromBody]RemovePlayer command)
            => await ProcessCommand(command);

        [HttpPut("changeowner")]
        public async Task<IActionResult> ChangeOwner([FromBody]ChangeOwner command)
            => await ProcessCommand(command);

        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePassword command)
            => await ProcessCommand(command);

        [HttpPut("changeprivacy")]
        public async Task<IActionResult> ChangePrivacy([FromBody]ChangePrivacy command)
            => await ProcessCommand(command);

        [HttpPut("startgame")]
        public async Task<IActionResult> StartGame([FromBody]StartGame command)
            => await ProcessCommand(command);
    }
}
