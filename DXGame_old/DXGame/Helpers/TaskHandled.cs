﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DXGame.Helpers
{
    public class TaskHandled : ITaskHandled
    {
        private readonly ITaskHandler _handler;
        private readonly Func<Task> _run;
        private Func<Task> _onSuccess;
        private Func<Exception, Task> _onError;
        private Func<Task> _finally;
        private bool _propagateException = true;
        private bool _executeOnError = true;
        private IDictionary<Type, Func<Exception, Task>> _onCustomErrors = new Dictionary<Type, Func<Exception, Task>>();

        public TaskHandled(ITaskHandler handler, Func<Task> run)
        {
            _handler = handler;
            _run = run;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                await _run();
                if (_onSuccess != null)
                {
                    await _onSuccess();
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex);
                if (_propagateException)
                {
                    throw;
                }
            }
            finally
            {
                if (_finally != null)
                {
                    await _finally();
                }
            }
        }

        public ITaskHandled OnSuccess(Func<Task> func)
        {
            _onSuccess = func;

            return this;
        }

        public ITaskHandled OnError(Func<Exception, Task> func, bool executeAlsoWithCustomError = true)
        {
            _onError = func;
            _executeOnError = executeAlsoWithCustomError;

            return this;
        }

        public ITaskHandled OnCustomError<T>(Func<Exception, Task> func) where T : Exception
        {
            var test = _onCustomErrors.Keys.SingleOrDefault(k => k == typeof(T));

            if (test != default(Type))
            {
                _onCustomErrors[test] = func;
            }
            else
            {
                _onCustomErrors.Add(typeof(T), func);
            }

            return this;
        }

        public ITaskHandled PropagateException()
        {
            _propagateException = true;

            return this;
        }

        public ITaskHandled DoNotPropagateException()
        {
            _propagateException = false;

            return this;
        }

        public ITaskHandled Finally(Func<Task> func)
        {
            _finally = func;

            return this;
        }

        public ITaskHandler Then()
        {
            return _handler;
        }

        private async Task HandleExceptionAsync(Exception ex)
        {
            var customException = _onCustomErrors.Keys.Any(k => k == ex.GetType());
            if (customException)
            {
                if (_onCustomErrors[ex.GetType()] != null)
                {
                    await _onCustomErrors[ex.GetType()](ex);
                }
            }

            var executeOnError = _executeOnError || !customException;
            if (executeOnError)
            {
                if (_onError != null)
                {
                    await _onError(ex);
                }
            }
        }
    }
}