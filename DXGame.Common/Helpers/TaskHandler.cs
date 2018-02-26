using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DXGame.Common.Helpers
{
    public class Handler : IHandler
    {
        private ISet<HandlerTask> _tasks = new HashSet<HandlerTask>();

        public async Task ExecuteAsync()
        {
            foreach (var task in _tasks)
            {
                await task.ExecuteAsync();
            }

            _tasks.Clear();
        }

        public IHandlerTask Run(Func<Task> func)
        {
            var task = new HandlerTask(this, func);
            _tasks.Add(task);

            return task;
        }
    }
}