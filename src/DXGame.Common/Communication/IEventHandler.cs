using System.Threading.Tasks;
using DXGame.Messages.Abstract;

namespace DXGame.Common.Communication
{
    public interface IEventHandler<T> : IMessageHandler<T> where T : IEvent
    {
        
    }
}