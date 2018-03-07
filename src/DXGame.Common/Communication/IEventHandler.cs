using System.Threading.Tasks;
using DXGame.Messages.Abstract;

namespace DXGame.Common.Communication
{
    public interface IEventHandler<T> where T : IEvent
    {
        Task HandleAsync(T @event); 
    }
}