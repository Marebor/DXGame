using System;
using System.Threading.Tasks;

namespace DXGame.Common.Helpers
{
    public interface IErrorHandler
    {
        Task ExecuteAsync();
        IErrorHandler OnError(Func<Exception, Task> func, bool executeAlsoWithCustomError = true);
        IErrorHandler OnCustomError<TError>(Func<TError, Task> func) where TError : Exception;
        IErrorHandler PropagateException();
        IErrorHandler DoNotPropagateException();
        IErrorHandler Finally(Func<Task> func);
        IHandler Then();
    }
}