using System.Threading.Tasks;

namespace DXGame.Common.Communication
{
    public interface IEventHandler<T> where T : IEvent
    {
        Task HandleAsync(T @event); 
    }
}