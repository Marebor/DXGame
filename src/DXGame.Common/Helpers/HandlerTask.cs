using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DXGame.Common.Models;

namespace DXGame.Common.Helpers
{
    public class HandlerTask<T> : IHandlerTask<T>
    {
        private readonly IHandler _handler;
        private readonly Func<Task<T>> _loadAggregate;
        private T _aggregate;
        private Action<T> _validate;
        private Action<T> _run;
        private Func<T, Task> _onSuccess;
        private Func<Exception, Task> _onError;
        private Func<Task> _finally;
        private bool _propagateException = true;
        private bool _executeOnError = true;
        private IDictionary<Type, Func<Exception, Task>> _onCustomErrors = new Dictionary<Type, Func<Exception, Task>>();

        public HandlerTask(IHandler handler, Func<Task<T>> loadAggregate)
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

        public IHandlerTask<T> Validate(Action<T> func)
        {
            _validate = func;

            return this;
        }

        public IHandlerTask<T> Run(Action<T> func)
        {
            _run = func;

            return this;
        }

        public IHandlerTask<T> OnSuccess(Func<T, Task> func)
        {
            _onSuccess = func;

            return this;
        }

        public IHandlerTask<T> OnError(Func<Exception, Task> func, bool executeAlsoWithCustomError = true)
        {
            _onError = func;
            _executeOnError = executeAlsoWithCustomError;

            return this;
        }

        public IHandlerTask<T> OnCustomError<TError>(Func<TError, Task> func) where TError : Exception
        {
            var test = _onCustomErrors.Keys.SingleOrDefault(k => k == typeof(TError));

            if (test != default(Type))
            {
                _onCustomErrors[test] = func as Func<Exception, Task>;
            }
            else
            {
                _onCustomErrors.Add(typeof(TError), func  as Func<Exception, Task>);
            }

            return this;
        }

        public IHandlerTask<T> PropagateException()
        {
            _propagateException = true;

            return this;
        }

        public IHandlerTask<T> DoNotPropagateException()
        {
            _propagateException = false;

            return this;
        }

        public IHandlerTask<T> Finally(Func<Task> func)
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