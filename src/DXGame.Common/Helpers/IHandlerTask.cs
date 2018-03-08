using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXGame.Common.Models;

namespace DXGame.Common.Helpers
{
    public interface IHandlerTask<T>
    {
        Task ExecuteAsync();
        IHandlerTask<T> Validate(Action<T> func);
        IHandlerTask<T> Run(Action<T> func);
        IHandlerTask<T> OnSuccess(Func<T, Task> func);
        IHandlerTask<T> OnError(Func<Exception, Task> func, bool executeAlsoWithCustomError = true);
        IHandlerTask<T> OnCustomError<TError>(Func<TError, Task> func) where TError : Exception;
        IHandlerTask<T> PropagateException();
        IHandlerTask<T> DoNotPropagateException();
        IHandlerTask<T> Finally(Func<Task> func);
        IHandler Then();
    }
}
