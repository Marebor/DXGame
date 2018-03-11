using System;
using System.Threading.Tasks;

namespace DXGame.Common.Helpers
{
    public interface IErrorHandler
    {
        Task ExecuteAsync();
        IErrorPropagator OnError(Func<Exception, Task> func, bool executeAlsoWithCustomError = true);
        IErrorPropagator OnCustomError<TError>(Func<TError, Task> func) where TError : Exception;
        IErrorHandler Finally(Func<Task> func);
        IHandler Then();
    }
}