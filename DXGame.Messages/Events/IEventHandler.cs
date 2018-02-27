using System.Threading.Tasks;

namespace DXGame.Messages.Events
{
    public interface IEventHandler<T> where T : IEvent
    {
        Task HandleAsync(T @event); 
    }
}