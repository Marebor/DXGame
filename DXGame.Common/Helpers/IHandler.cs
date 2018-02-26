using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXGame.Common.Helpers
{
    public interface IHandler
    {
        IHandlerTask Run(Func<Task> func);
        Task ExecuteAsync();
    }
}
