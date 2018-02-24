using System;

namespace DXGame.Core.Domain.Exceptions 
{
    public abstract class DXGameException : Exception 
    {
        public string ErrorCode { get; protected set; }

        public DXGameException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}