using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXGame.Helpers
{
    public interface ITaskHandler
    {
        ITaskHandled Run(Func<Task> func);
        Task ExecuteAsync();
    }
}
