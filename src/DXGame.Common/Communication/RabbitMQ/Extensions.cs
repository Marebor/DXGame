using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DXGame.Common.Helpers;
using DXGame.Messages.Commands;
using DXGame.Messages.Events;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Instantiation;
using RawRabbit.Pipe;

namespace DXGame.Common.Communication.RabbitMQ
{
    public static class Extensions
    {
        public static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new RabbitMQSettings();
            var section = configuration.GetSection("rabbitmq");
            section.Bind(settings);
            var client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions
            {
                ClientConfiguration = settings
            });
            services.AddSingleton<IBusClient>(_ => client);
        }

        public static void AddSubscriptionsForMessageHandlers(this IBusClient busClient, IServiceProvider serviceProvider)
        {
            ForEachMessageHandlerInAssembly.Execute(
                (handlerType, handlerInterface, messageType) =>
                {
                    var handler = serviceProvider.GetService(handlerInterface);
                    busClient.SubscribeToMessage(messageType, handler);
                },
                Assembly.GetCallingAssembly()
            );
        }

        static void SubscribeToMessage(this IBusClient busClient, Type msgType, object handler)
        {
            var subscribeMethod = BusClientSubscriptionMethod(msgType);
            subscribeMethod.Invoke(null, new object[] 
            {
                busClient,
                msgType.IsAssignableTo<ICommand>() ? 
                    CommandHandlerHandleAsyncMethod(handler) as object : EventHandlerHandleAsyncMethod(handler),
                SubscriptionContext(msgType, handler.GetType()),
                default(CancellationToken)
            });
        }

        static MethodInfo BusClientSubscriptionMethod(Type genericType)
            => typeof(SubscribeMessageExtension)
                .GetMethods()
                .First(m => m.Name.Contains("SubscribeAsync"))
                .MakeGenericMethod(genericType);

        static Func<ICommand, Task> CommandHandlerHandleAsyncMethod(object handler)
            => msg => (handler as ICommandHandler<ICommand>).HandleAsync(msg);
        
        static Func<IEvent, Task> EventHandlerHandleAsyncMethod(object handler)
            => msg => (handler as IEventHandler<IEvent>).HandleAsync(msg);

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