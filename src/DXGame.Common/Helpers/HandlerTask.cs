using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DXGame.Common.Models;

namespace DXGame.Common.Helpers
{
    public class HandlerTask<T> : IHandlerTask<T>, IErrorHandler, IErrorPropagator
    {
        private readonly IHandler _handler;
        private readonly Func<Task<T>> _loadAggregate;
        private T _aggregate;
        private Action<T> _validate;
        private Action<T> _run;
        private Action _runNoResult;
        private Func<T, Task> _onSuccess;
        private ErrorHandling _onError;
        private Func<Task> _finally;
        private bool _executeOnError = true;
        private IDictionary<Type, ErrorHandling> _onCustomErrors = new Dictionary<Type, ErrorHandling>();
        private ErrorHandling _currentErrorDefinition;

        public HandlerTask(IHandler handler, Func<Task<T>> loadAggregate)
        {
            _handler = handler;
            _loadAggregate = loadAggregate;
        }

        public HandlerTask(IHandler handler, Action runNoResult)
        {
            _handler = handler;
            _runNoResult = runNoResult;
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
                if (_runNoResult != null) 
                {
                    _runNoResult();
                }
                if (_onSuccess != null)
                {
                    await _onSuccess(_aggregate);
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex);
                var propagate = _onCustomErrors.ContainsKey(ex.GetType()) ?
                    _onCustomErrors[ex.GetType()].Propagate : _onError.Propagate;
                if (propagate)
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

        public IErrorHandler OnSuccess(Func<T, Task> func)
        {
            _onSuccess = func;

            return this;
        }

        public IErrorPropagator OnError(Func<Exception, Task> func, bool executeAlsoWithCustomError = true)
        {
            _onError = new ErrorHandling(typeof(Exception), func, true);
            _currentErrorDefinition = _onError;
            _executeOnError = executeAlsoWithCustomError;

            return this;
        }

        public IErrorPropagator OnCustomError<TError>(Func<TError, Task> func) where TError : Exception
        {
            var errorHandling = new ErrorHandling(typeof(TError), func as Func<Exception, Task>, true);
            _currentErrorDefinition = errorHandling;

            var test = _onCustomErrors.Keys.SingleOrDefault(k => k == typeof(TError));

            if (test != default(Type))
            {
                _onCustomErrors[test] = errorHandling;
            }
            else
            {
                _onCustomErrors.Add(typeof(TError), errorHandling);
            }

            return this;
        }

        public IErrorHandler PropagateException()
        {
            _currentErrorDefinition.Propagate = true;

            return this;
        }

        public IErrorHandler DoNotPropagateException()
        {
            _currentErrorDefinition.Propagate = false;

            return this;
        }

        public IErrorHandler Finally(Func<Task> func)
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
                    await _onCustomErrors[ex.GetType()].Handler(ex);
                }
            }

            var executeOnError = _executeOnError || !customException;
            if (executeOnError)
            {
                if (_onError != null)
                {
                    await _onError.Handler(ex);
                }
            }
        }

        private class ErrorHandling
        {
            public Type ExceptionType { get; set; }
            public Func<Exception, Task> Handler { get; set; }
            public bool Propagate { get; set; }

            public ErrorHandling(Type exceptionType, Func<Exception, Task> handler, bool propagate)
            {
                ExceptionType = exceptionType;
                Handler = handler;
                Propagate = propagate;
            }
        }
    }
}