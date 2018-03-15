using System;
using System.Threading.Tasks;
using DXGame.Messages.Abstract;

namespace DXGame.Common.Communication
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(T message) where T : IMessage;

        Task SubscribeAsync<T>(IMessageHandler<T> handler) where T : IMessage;
    }
}