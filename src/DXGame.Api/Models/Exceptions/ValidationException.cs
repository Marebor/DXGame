using DXGame.Common.Exceptions;

namespace DXGame.Api.Models.Exceptions
{
    public class ValidationException : DXGameException
    {
        public ValidationException(string errorCode, string message = null) : base(errorCode, message)
        {
        }
    }
}