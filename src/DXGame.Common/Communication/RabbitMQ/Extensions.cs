using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
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
            var assembly = Assembly.GetCallingAssembly();
            var handlers = assembly
                .GetTypes()
                .Where(t => t
                    .GetInterfaces()
                    .Any(i => 
                        i.IsClosedTypeOf(typeof(ICommandHandler<>)) ||
                        i.IsClosedTypeOf(typeof(IEventHandler<>))
                    )
                );

            foreach (var handlerType in handlers)
            {
                var handlerInterface = handlerType
                    .GetInterfaces()
                    .First(i => 
                        i.IsClosedTypeOf(typeof(ICommandHandler<>)) ||
                        i.IsClosedTypeOf(typeof(IEventHandler<>))                        
                    );
                var msgType = handlerInterface
                    .GetGenericArguments()
                    .First();
                var handler = serviceProvider.GetService(handlerInterface);
                busClient.SubscribeToMessage(msgType, handler);
            }
        }

        static void SubscribeToMessage(this IBusClient busClient, Type msgType, object handler)
        {
            var subscribeMethod = BusClientSubscriptionMethod(msgType);
            subscribeMethod.Invoke(null, new object[] 
            {
                busClient,
                msgType.IsAssignableTo<ICommand>() ? 
                    CommandHandlerHandleAsyncMethod(handler) as object : EventHandlerHandleAsyncMethod(handler),
                SubscriptionContext(msgType),
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

        static Action<IPipeContext> SubscriptionContext(Type msgType)
            => ctx => ctx.UseConsumerConfiguration(
                            cfg => cfg.FromDeclaredQueue(
                                q => q.WithName(QueueName(msgType))
                            )
                        );

        static string QueueName(Type msgType, string suffix = null)
            => $"{Assembly.GetEntryAssembly().GetName()}/{msgType.Name}" + suffix != null ? "_{suffix}" : string.Empty;
    }
}