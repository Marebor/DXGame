using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Api.Models;
using DXGame.Api.Models.Exceptions;
using DXGame.Common.Communication;
using DXGame.Messages.Commands.Playroom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Controllers
{
    [Route("api/[controller]")]
    public class PlayroomsController : Controller
    {
        IActionResultHelper _actionResultHelper;
        ICache _cache;
        ILogger _logger;
        IMessageBus _messageBus;

        public PlayroomsController(IActionResultHelper actionResultHelper, ICache cache, 
            ILogger logger, IMessageBus messageBus)
        {
            _actionResultHelper = actionResultHelper;
            _cache = cache;
            _logger = logger;
            _messageBus = messageBus;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
            => await _actionResultHelper
                .Return(async () => 
                {
                    var playrooms = await _cache.GetAsync<ISet<Guid>>(typeof(PlayroomDto).Name);
                    return Ok(playrooms);
                })
                .OnError(ex => NotFound())
                .DoNotPropagateException()
                .ExecuteAsync();

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
            => await _actionResultHelper
                .Return(async () => 
                {
                    var playroom = await _cache.GetAsync<PlayroomDto>(id);
                    return Ok(playroom);
                })
                .OnError(ex => NotFound())
                .DoNotPropagateException()
                .ExecuteAsync();

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreatePlayroom command)
            => await _actionResultHelper
                .Return(async () => 
                {
                    await _messageBus.PublishAsync(command);
                    return Accepted();
                })
                .OnError(ex => NotFound())
                .DoNotPropagateException()
                .ExecuteAsync();

        // PUT api/values/5
        [HttpPut]
        public async Task<IActionResult> AddPlayer([FromBody]AddPlayer command)
            => await _actionResultHelper
                .Return(async () => 
                {
                    await _messageBus.PublishAsync(command);
                    return Accepted();
                })
                .OnError(ex => NotFound())
                .DoNotPropagateException()
                .ExecuteAsync();

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, [FromBody]DeletePlayroom command)
            => await _actionResultHelper
                .Validate(() => command.Playroom == id)
                .Return(async () => 
                {
                    await _messageBus.PublishAsync(command);
                    return Accepted();
                })
                .OnCustomError<ValidationException>(ex => BadRequest(ex.ErrorCode))
                .OnError(ex => NotFound())
                .DoNotPropagateException()
                .ExecuteAsync();
    }
}
