using DXGame.Common.Communication;

namespace DXGame.Common.Models
{
    public interface IApplyEvent<T> where T : IEvent
    {
         void ApplyEvent(T e);
    }
}