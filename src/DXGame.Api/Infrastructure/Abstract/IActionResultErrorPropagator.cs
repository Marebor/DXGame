namespace DXGame.Api.Infrastructure.Abstract
{
    public interface IActionResultErrorPropagator
    {
        IActionResultErrorHandler PropagateException();
        IActionResultErrorHandler DoNotPropagateException();
    }
}