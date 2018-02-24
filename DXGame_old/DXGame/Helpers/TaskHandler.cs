using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DXGame.Helpers
{
    public class TaskHandler : ITaskHandler
    {
        private ISet<ITaskHandled> _tasks = new HashSet<ITaskHandled>();

        public async Task ExecuteAsync()
        {
            foreach (var task in _tasks)
            {
                await task.ExecuteAsync();
            }

            _tasks.Clear();
        }

        public ITaskHandled Run(Func<Task> func)
        {
            var task = new TaskHandled(this, func);
            _tasks.Add(task);

            return task;
        }
    }
}