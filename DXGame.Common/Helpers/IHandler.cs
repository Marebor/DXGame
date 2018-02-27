using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXGame.Common.Models;

namespace DXGame.Common.Helpers
{
    public interface IHandler
    {
        IHandlerTask LoadAggregate(Func<Task<Aggregate>> func);
        Task ExecuteAsync();
    }
}
