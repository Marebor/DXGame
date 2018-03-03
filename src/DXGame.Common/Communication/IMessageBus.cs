using System;
using System.Threading.Tasks;

namespace DXGame.Common.Communication
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(T message) where T : IMessage;

        Task SubscribeAsync<T>(Func<T, Task> handler) where T : IMessage;
    }
}