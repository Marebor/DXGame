namespace DXGame.Core.Domain.Exceptions 
{
    public class DomainException : DXGameException
    {
        public DomainException(string errorCode) : base(errorCode, null) 
        {
        }

        public DomainException(string errorCode, string message) : base(errorCode, message) 
        {
        }
    }
}