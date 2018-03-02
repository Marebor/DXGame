using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DXGame.Messages.Commands;
using DXGame.Messages.Events;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Context;

namespace DXGame.Common.Communication.RabbitMQ
{
    public static class Extensions
    {
        public static void AddRegisteredMessageHandlers(this IBusClient busClient, IServiceProvider serviceProvider)
        {
            var assembly = Assembly.GetCallingAssembly();
            
            AddHandlers(assembly, typeof(IEventHandler<>), busClient, serviceProvider);
            AddHandlers(assembly, typeof(ICommandHandler<>), busClient, serviceProvider);
        }

        static void AddHandlers(Assembly assembly, Type type, IBusClient busClient, IServiceProvider serviceProvider)
        {
            var handlerName = type.Name;
            var handlers = assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.Name.Contains(handlerName)));

            foreach (var handler in handlers)
            {
                var handlerInterface = handler
                    .GetInterfaces()
                    .First(i => i.Name.Contains(handlerName));
                var msgType = handlerInterface
                    .GetGenericArguments()
                    .First();
                var subscribeMethod = typeof(IBusClient)
                    .GetInterfaces()
                    .Single(i => i.GetMethods()
                        .Any(m => m.Name
                            .Contains("SubscribeAsync") && m.IsGenericMethod
                        )
                    )
                    .GetMethods()
                    .First(m => m.Name.Contains("SubscribeAsync"))
                    .MakeGenericMethod(msgType);
                var instance = serviceProvider.GetService(handlerInterface);

                if (type.Name.Contains(typeof(ICommandHandler<>).Name)) 
                    SubscribeToCommand(subscribeMethod, busClient, instance);
                if (type.Name.Contains(typeof(IEventHandler<>).Name)) 
                    SubscribeToEvent(subscribeMethod, busClient, instance);
            }
        }

        static void SubscribeToCommand(MethodInfo subscribeMethod, IBusClient busClient, object handler)
        {
            subscribeMethod.Invoke(busClient, new object[] 
            {
                new Func<ICommand, MessageContext, Task>((msg, ctx) 
                    => (handler as ICommandHandler<ICommand>).HandleAsync(msg)),
                null
            });
        }

        static void SubscribeToEvent(MethodInfo subscribeMethod, IBusClient busClient, object handler)
        {
            subscribeMethod.Invoke(busClient, new object[] 
            {
                new Func<IEvent, MessageContext, Task>((msg, ctx) 
                    => (handler as IEventHandler<IEvent>).HandleAsync(msg)),
                null
            });
        }
    }
}