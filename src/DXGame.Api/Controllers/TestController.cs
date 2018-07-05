using System;
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
    public class TestController : ExtendedController
    {
        IActionResultHelper _actionResultHelper;
        ILogger<TestController> _logger;
        IMessageBus _messageBus;
        IProjectionRepository _projectionRepository;

        public TestController(IActionResultHelper actionResultHelper, ILogger<TestController> logger, 
            IMessageBus messageBus, IProjectionRepository projectionRepository)
            : base(actionResultHelper, logger, messageBus)
        {
            _actionResultHelper = actionResultHelper;
            _logger = logger;
            _messageBus = messageBus;
            _projectionRepository = projectionRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
            => await _actionResultHelper
                .Return(async () => 
                {
                    var playrooms = await _projectionRepository.BrowseAsync<PlayroomProjection>();
                    return Ok(playrooms);
                })
                .OnError(ex => 
                { 
                    _logger.LogError(ex, ex.Message); 
                    return InternalServerError(); 
                })
                .PropagateException()
                .ExecuteAsync();

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]string name)
            => await _actionResultHelper
                .Return(async () => 
                {
                    _logger.LogWarning($"Executing test Post action with argument: {name}");
                    var command = new CreatePlayroom(
                        Guid.NewGuid(), Guid.NewGuid(), name, Guid.NewGuid(), false, "secret"
                    );
                    _logger.LogWarning("Attempting to send command...");
                    await _messageBus.PublishAsync(command);
                    _logger.LogWarning("Message published.");
                    await _projectionRepository.SaveAsync(new PlayroomProjection { Name = name });
                    _logger.LogWarning("Playroom projection saved.");
                    return Accepted();
                })
                .OnError(ex => 
                { 
                    _logger.LogError(ex, ex.Message); 
                    return ServiceUnavailable(); 
                })
                .DoNotPropagateException()
                .ExecuteAsync();
    }
}