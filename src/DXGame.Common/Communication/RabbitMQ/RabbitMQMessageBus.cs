using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DXGame.Common.Infrastructure.Abstract;
using DXGame.Messages.Abstract;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DXGame.Common.Communication.RabbitMQ
{
    public class RabbitMQMessageBus : IMessageBus
    {
        IBinarySerializer _serializer;
        IModel _bus;
        EventingBasicConsumer _consumer;

        public RabbitMQMessageBus(IModel bus, IBinarySerializer serializer, EventingBasicConsumer consumer)
        {
            _bus = bus;
            _consumer = consumer;
            _serializer = serializer;
        }
        
        public async Task PublishAsync<T>(T message) where T : IMessage
            => await Task.Run(() => 
                        {
                            var exchangeName = typeof(T).Name;
                            _bus.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
                            _bus.BasicPublish(exchange: exchangeName,
                                            routingKey: "",
                                            basicProperties: null,
                                            body: _serializer.Serialize(message));
                        });

        public async Task SubscribeAsync<T>(Func<T, Task> handler) where T : IMessage
            => await Task.Run(() => 
                        {
                            var queueName = QueueName(typeof(T), handler.GetType());
                            var exchangeName = typeof(T).Name;
                            _bus.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
                            _bus.QueueDeclare(queue: queueName,
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);
                            _bus.QueueBind(queue: queueName,
                                        exchange: exchangeName,
                                        routingKey: "");
                            _consumer.Received += (model, ea) =>
                            {
                                var message = _serializer.Deserialize<T>(ea.Body);
                                handler(message);
                            };
                        });

        static string QueueName(Type msgType, Type handler)
            => $"{Assembly.GetEntryAssembly().GetName()}/{msgType.Name}/{handler.FullName}";
    }
}