using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXGame.Helpers
{
    public interface ITaskHandled
    {
        Task ExecuteAsync();
        ITaskHandled OnSuccess(Func<Task> func);
        ITaskHandled OnError(Func<Exception, Task> func, bool executeAlsoWithCustomError = true);
        ITaskHandled OnCustomError<T>(Func<Exception, Task> func) where T : Exception;
        ITaskHandled PropagateException();
        ITaskHandled DoNotPropagateException();
        ITaskHandled Finally(Func<Task> func);
        ITaskHandler Then();
    }
}
