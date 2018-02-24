namespace DXGame.Core.Domain.Exceptions 
{
    public class DomainException : DXGameException
    {
        public DomainException(string errorCode) : base(errorCode) 
        {
            
        }
    }
}