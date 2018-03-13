using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DXGame.Api.Infrastructure
{
    public class ActionResultHelper : IActionResultHelper, IActionResultErrorHandler, IActionResultErrorPropagator
    {
        private Func<Task<IActionResult>> _return;
        private IDictionary<Type, ErrorHandling> _onCustomErrors = new Dictionary<Type, ErrorHandling>();
        private ErrorHandling _onError;
        private ErrorHandling _currentErrorDefinition;
        private Func<Task> _finally;

        public async Task<IActionResult> ExecuteAsync()
        {
            try
            {
                if (_return != null) 
                {
                    return await _return();
                }
            }
            catch (Exception ex)
            {
                var result = HandleExceptionAsync(ex);
                var propagate = _onCustomErrors.ContainsKey(ex.GetType()) ?
                    _onCustomErrors[ex.GetType()].Propagate : _onError.Propagate;
                if (propagate)
                {
                    throw;
                }
                return result;
            }
            finally
            {
                if (_finally != null)
                {
                    await _finally();
                }
            }

            return new StatusCodeResult(500);
        }

        public IActionResultErrorHandler Return<T>(Func<Task<T>> action) where T : IActionResult
        {
            _return = action as Func<Task<IActionResult>>;

            return this;
        }

        public IActionResultErrorPropagator OnCustomError<TError>(Func<TError, IActionResult> func) where TError : Exception
        {
            var errorHandling = new ErrorHandling(typeof(TError), func as Func<Exception, IActionResult>, true);
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

        public IActionResultErrorPropagator OnError(Func<Exception, IActionResult> func)
        {
            _onError = new ErrorHandling(typeof(Exception), func, true);
            _currentErrorDefinition = _onError;

            return this;
        }

        public IActionResultErrorHandler PropagateException()
        {
            _currentErrorDefinition.Propagate = true;

            return this;
        }

        public IActionResultErrorHandler DoNotPropagateException()
        {
            _currentErrorDefinition.Propagate = false;

            return this;
        }

        public IActionResultErrorHandler Finally(Func<Task> func)
        {
            _finally = func;

            return this;
        }

        private IActionResult HandleExceptionAsync(Exception ex)
        {
            var customException = _onCustomErrors.Keys.Any(k => k == ex.GetType());
            if (customException)
            {
                if (_onCustomErrors[ex.GetType()] != null)
                {
                    return _onCustomErrors[ex.GetType()].Handler(ex);
                }
            }

            if (_onError != null)
            {
                return _onError.Handler(ex);
            }

            return new StatusCodeResult(500);
        }

        private class ErrorHandling
        {
            public Type ExceptionType { get; set; }
            public Func<Exception, IActionResult> Handler { get; set; }
            public bool Propagate { get; set; }

            public ErrorHandling(Type exceptionType, Func<Exception, IActionResult> handler, bool propagate)
            {
                ExceptionType = exceptionType;
                Handler = handler;
                Propagate = propagate;
            }
        }
    }
}