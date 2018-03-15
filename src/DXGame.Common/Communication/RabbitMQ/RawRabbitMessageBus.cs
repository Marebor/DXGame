using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DXGame.Messages.Abstract;
using RawRabbit;
using RawRabbit.Pipe;

namespace DXGame.Common.Communication.RabbitMQ
{
    public class RawRabbitMessageBus : IMessageBus
    {
        IBusClient _bus;

        public RawRabbitMessageBus(IBusClient bus)
        {
            _bus = bus;
        }
        
        public async Task PublishAsync<T>(T message) where T : IMessage
            => await _bus.PublishAsync(message);

        public async Task SubscribeAsync<T>(IMessageHandler<T> handler) where T : IMessage
            => await _bus.SubscribeAsync<T>(
                            t => handler.HandleAsync(t), 
                            SubscriptionContext(typeof(T), handler.GetType())
                        );

        static Action<IPipeContext> SubscriptionContext(Type msgType, Type handler)
            => ctx => ctx.UseConsumerConfiguration(
                            cfg => cfg.FromDeclaredQueue(
                                q => q.WithName(QueueName(msgType, handler))
                            )
                        );

        static string QueueName(Type msgType, Type handler)
            => $"{Assembly.GetEntryAssembly().GetName()}/{msgType.Name}/{handler.Name}";
    }
}