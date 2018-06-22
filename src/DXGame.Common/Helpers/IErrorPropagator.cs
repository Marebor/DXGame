namespace DXGame.Common.Helpers
{
    public interface IErrorPropagator
    {
        IErrorHandler PropagateException();
        IErrorHandler DoNotPropagateException();
    }
}