using System;
using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Common.Communication;
using DXGame.Common.Exceptions;
using DXGame.Messages.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DXGame.Api.Controllers
{
    public abstract class ExtendedController : Controller
    {
        IActionResultHelper _actionResultHelper;
        ILogger _logger;
        IMessageBus _messageBus;

        public ExtendedController(IActionResultHelper actionResultHelper, ILogger logger, IMessageBus messageBus)
        {
            _actionResultHelper = actionResultHelper;
            _logger = logger;
            _messageBus = messageBus;
        }
        
        public async Task<IActionResult> Return<T>(Func<Task<T>> returns) 
            => await _actionResultHelper
                .Return(async () => 
                {
                    return Ok(await returns());
                })
                .OnError(ex => 
                { 
                    _logger?.LogError(ex, ex.Message); 
                    return NotFound(); 
                })
                .DoNotPropagateException()
                .ExecuteAsync();

        public async Task<IActionResult> ProcessCommand(ICommand command)
            => await _actionResultHelper
                .Return(async () => 
                {
                    await _messageBus.PublishAsync(command);
                    return Accepted();
                })
                .OnError(ex => 
                { 
                    _logger.LogError(ex, ex.Message); 
                    return ServiceUnavailable(); 
                })
                .DoNotPropagateException()
                .ExecuteAsync();

        public IActionResult ServiceUnavailable() 
        {
            return new StatusCodeResult(503);
        }

        public IActionResult InternalServerError()
        {
            return new StatusCodeResult(500);
        }
    }
}