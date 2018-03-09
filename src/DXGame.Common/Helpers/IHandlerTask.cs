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
        IErrorHandler OnSuccess(Func<T, Task> func);
        IHandler Then();
    }
}
