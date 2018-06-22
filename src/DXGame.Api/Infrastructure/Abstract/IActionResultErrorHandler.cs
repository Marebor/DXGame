using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DXGame.Api.Infrastructure.Abstract
{
    public interface IActionResultErrorHandler
    {
        Task<IActionResult> ExecuteAsync();
        IActionResultErrorPropagator OnError(Func<Exception, IActionResult> func);
        IActionResultErrorPropagator OnCustomError<TError>(Func<TError, IActionResult> func) where TError : Exception;
        IActionResultErrorHandler Finally(Func<Task> func);
    }
}