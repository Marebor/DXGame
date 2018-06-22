using System.Threading.Tasks;
using DXGame.Messages.Abstract;

namespace DXGame.Common.Communication
{
    public interface IMessageHandler<T> where T : IMessage
    {
        Task HandleAsync(T message);
    }
}