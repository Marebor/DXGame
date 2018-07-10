using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DXGame.Common.Models;
using Microsoft.Extensions.Logging;

namespace DXGame.Common.Helpers
{
    public class Handler : IHandler
    {
        private readonly ILogger _logger;
        private ISet<HandlerTask<object>> _tasks = new HashSet<HandlerTask<object>>();

        public Handler(ILogger<Handler> logger)
        {
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            foreach (var task in _tasks)
            {
                await task.ExecuteAsync();
            }

            _tasks.Clear();
        }

        public IErrorHandler Run(Action action)
        {
            return new HandlerTask<object>(this, action);
        }

        public IHandlerTask<T> LoadAggregate<T>(Func<Task<T>> func)
        {
            var task = new HandlerTask<T>(this, func, _logger);
            _tasks.Add(task as HandlerTask<object>);

            return task;
        }
    }
}