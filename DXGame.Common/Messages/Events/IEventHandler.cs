using System.Threading.Tasks;

namespace DXGame.Common.Messages.Events
{
    public interface IEventHandler<T> where T : IEvent
    {
        Task HandleAsync(T @event); 
    }
}