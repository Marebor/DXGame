using DXGame.Common.Messages.Events;

namespace DXGame.Common.Models
{
    public interface IApplyEvent<T> where T : IEvent
    {
         void ApplyEvent(T e);
    }
}