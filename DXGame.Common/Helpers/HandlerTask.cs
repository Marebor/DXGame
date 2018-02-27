using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DXGame.Common.Models;

namespace DXGame.Common.Helpers
{
    public class HandlerTask : IHandlerTask
    {
        private readonly IHandler _handler;
        private readonly Func<Task<Aggregate>> _loadAggregate;
        private Aggregate _aggregate;
        private Action<Aggregate> _validate;
        private Action<Aggregate> _run;
        private Func<Aggregate, Task> _onSuccess;
        private Func<Exception, Task> _onError;
        private Func<Task> _finally;
        private bool _propagateException = true;
        private bool _executeOnError = true;
        private IDictionary<Type, Func<Exception, Task>> _onCustomErrors = new Dictionary<Type, Func<Exception, Task>>();

        public HandlerTask(IHandler handler, Func<Task<Aggregate>> loadAggregate)
        {
            _handler = handler;
            _loadAggregate = loadAggregate;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                if (_loadAggregate != null) 
                {
                    _aggregate = await _loadAggregate();
                }
                if (_validate != null) 
                {
                    _validate(_aggregate);
                }
                if (_run != null) 
                {
                    _run(_aggregate);
                }
                if (_onSuccess != null)
                {
                    await _onSuccess(_aggregate);
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

        public IHandlerTask Validate(Action<Aggregate> func)
        {
            _validate = func;

            return this;
        }

        public IHandlerTask Run(Action<Aggregate> func)
        {
            _run = func;

            return this;
        }

        public IHandlerTask OnSuccess(Func<Aggregate, Task> func)
        {
            _onSuccess = func;

            return this;
        }

        public IHandlerTask OnError(Func<Exception, Task> func, bool executeAlsoWithCustomError = true)
        {
            _onError = func;
            _executeOnError = executeAlsoWithCustomError;

            return this;
        }

        public IHandlerTask OnCustomError<T>(Func<T, Task> func) where T : Exception
        {
            var test = _onCustomErrors.Keys.SingleOrDefault(k => k == typeof(T));

            if (test != default(Type))
            {
                _onCustomErrors[test] = func as Func<Exception, Task>;
            }
            else
            {
                _onCustomErrors.Add(typeof(T), func  as Func<Exception, Task>);
            }

            return this;
        }

        public IHandlerTask PropagateException()
        {
            _propagateException = true;

            return this;
        }

        public IHandlerTask DoNotPropagateException()
        {
            _propagateException = false;

            return this;
        }

        public IHandlerTask Finally(Func<Task> func)
        {
            _finally = func;

            return this;
        }

        public IHandler Then()
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