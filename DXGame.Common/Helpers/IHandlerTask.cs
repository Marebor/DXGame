using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXGame.Common.Helpers
{
    public interface IHandlerTask
    {
        Task ExecuteAsync();
        IHandlerTask OnSuccess(Func<Task> func);
        IHandlerTask OnError(Func<Exception, Task> func, bool executeAlsoWithCustomError = true);
        IHandlerTask OnCustomError<T>(Func<T, Task> func) where T : Exception;
        IHandlerTask PropagateException();
        IHandlerTask DoNotPropagateException();
        IHandlerTask Finally(Func<Task> func);
        IHandler Then();
    }
}
