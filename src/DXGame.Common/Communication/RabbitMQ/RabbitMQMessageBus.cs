using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using RawRabbit;
using RawRabbit.Pipe;

namespace DXGame.Common.Communication.RabbitMQ
{
    public class RabbitMQMessageBus : IMessageBus
    {
        IBusClient _bus;

        public RabbitMQMessageBus(IBusClient bus)
        {
            _bus = bus;
        }
        
        public async Task PublishAsync<T>(T message) where T : IMessage
            => await _bus.PublishAsync(message);

        public async Task SubscribeAsync<T>(Func<T, Task> handler) where T : IMessage
            => await _bus.SubscribeAsync<T>(
                            handler, 
                            SubscriptionContext(typeof(T), handler.GetType()),
                            default(CancellationToken)
                        );

        static Action<IPipeContext> SubscriptionContext(Type msgType, Type handler)
            => ctx => ctx.UseConsumerConfiguration(
                            cfg => cfg.FromDeclaredQueue(
                                q => q.WithName(QueueName(msgType, handler))
                            )
                        );

        static string QueueName(Type msgType, Type handler)
            => $"{Assembly.GetEntryAssembly().GetName()}/{msgType.Name}/{handler.FullName}";
    }
}