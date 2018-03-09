using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DXGame.Api.Infrastructure.Abstract
{
    public interface IActionResultErrorHandler
    {
        Task<IActionResult> ExecuteAsync();
        IActionResultErrorHandler OnError(Func<Exception, IActionResult> func, bool executeAlsoWithCustomError = true);
        IActionResultErrorHandler OnCustomError<TError>(Func<TError, IActionResult> func) where TError : Exception;
        IActionResultErrorHandler PropagateException();
        IActionResultErrorHandler DoNotPropagateException();
        IActionResultErrorHandler Finally(Func<Task> func);
    }
}