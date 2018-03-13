using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Api.Models;
using DXGame.Common.Communication;
using DXGame.Messages.Commands.Playroom;
using DXGame.ReadModel.Infrastructure.Abstract;
using DXGame.ReadModel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Controllers
{
    [Route("api/[controller]")]
    public class PlayroomsController : ExtendedController
    {
        IActionResultHelper _actionResultHelper;
        ILogger _logger;
        IMessageBus _messageBus;
        IProjectionService _projectionService;

        public PlayroomsController(IActionResultHelper actionResultHelper, ILogger logger, 
            IMessageBus messageBus, IProjectionService projectionService)
        {
            _actionResultHelper = actionResultHelper;
            _logger = logger;
            _messageBus = messageBus;
            _projectionService = projectionService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
            => await _actionResultHelper
                .Return(async () => 
                {
                    var playrooms = await _projectionService.BrowseAsync<PlayroomProjection>();
                    return Ok(playrooms);
                })
                .OnError(ex => InternalServerError())
                .DoNotPropagateException()
                .ExecuteAsync();

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
            => await _actionResultHelper
                .Return(async () => 
                {
                    var playroom = await _projectionService.GetAsync<PlayroomProjection>(id);
                    return Ok(playroom);
                })
                .OnError(ex => NotFound())
                .DoNotPropagateException()
                .ExecuteAsync();

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreatePlayroom command)
            => await _actionResultHelper
                .Return(async () => 
                {
                    await _messageBus.PublishAsync(command);
                    return Accepted();
                })
                .OnError(ex => ServiceUnavailable())
                .DoNotPropagateException()
                .ExecuteAsync();

        [HttpPut]
        public async Task<IActionResult> Delete([FromBody]DeletePlayroom command)
            => await _actionResultHelper
                .Return(async () => 
                {
                    await _messageBus.PublishAsync(command);
                    return Accepted();
                })
                .OnError(ex => ServiceUnavailable())
                .DoNotPropagateException()
                .ExecuteAsync();

        [HttpPut]
        public async Task<IActionResult> AddPlayer([FromBody]AddPlayer command)
            => await _actionResultHelper
                .Return(async () => 
                {
                    await _messageBus.PublishAsync(command);
                    return Accepted();
                })
                .OnError(ex => ServiceUnavailable())
                .DoNotPropagateException()
                .ExecuteAsync();

        [HttpPut]
        public async Task<IActionResult> RemovePlayer([FromBody]RemovePlayer command)
            => await _actionResultHelper
                .Return(async () => 
                {
                    await _messageBus.PublishAsync(command);
                    return Accepted();
                })
                .OnError(ex => ServiceUnavailable())
                .DoNotPropagateException()
                .ExecuteAsync();

        [HttpPut]
        public async Task<IActionResult> ChangeOwner([FromBody]ChangeOwner command)
            => await _actionResultHelper
                .Return(async () => 
                {
                    await _messageBus.PublishAsync(command);
                    return Accepted();
                })
                .OnError(ex => ServiceUnavailable())
                .DoNotPropagateException()
                .ExecuteAsync();

        [HttpPut]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePassword command)
            => await _actionResultHelper
                .Return(async () => 
                {
                    await _messageBus.PublishAsync(command);
                    return Accepted();
                })
                .OnError(ex => ServiceUnavailable())
                .DoNotPropagateException()
                .ExecuteAsync();

        [HttpPut]
        public async Task<IActionResult> ChangePrivacy([FromBody]ChangePrivacy command)
            => await _actionResultHelper
                .Return(async () => 
                {
                    await _messageBus.PublishAsync(command);
                    return Accepted();
                })
                .OnError(ex => ServiceUnavailable())
                .DoNotPropagateException()
                .ExecuteAsync();

        [HttpPut]
        public async Task<IActionResult> StartGame([FromBody]StartGame command)
            => await _actionResultHelper
                .Return(async () => 
                {
                    await _messageBus.PublishAsync(command);
                    return Accepted();
                })
                .OnError(ex => ServiceUnavailable())
                .DoNotPropagateException()
                .ExecuteAsync();
    }
}
