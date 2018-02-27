using System;

namespace DXGame.Common.Exceptions 
{
    public class DXGameException : Exception 
    {
        public string ErrorCode { get; protected set; }

        public DXGameException(string errorCode, string message = null) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}