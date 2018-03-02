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
            var assembly = Assembly.GetEntryAssembly();
            
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
                var msgType = handler
                    .GetGenericArguments()
                    .First();
                var subscribeMethod = typeof(IBusClient)
                    .GetMethod(nameof(IBusClient.SubscribeAsync))
                    .MakeGenericMethod(msgType);
                var handlerInterface = handler
                    .GetInterfaces()
                    .First(i => i.Name.Contains(handlerName));
                var instance = serviceProvider.GetService(handlerInterface);

                if (type.Name.Contains("Command")) 
                    SubscribeToCommand(subscribeMethod, busClient, instance);
                if (type.Name.Contains("Event")) 
                    SubscribeToEvent(subscribeMethod, busClient, instance);
            }
        }

        static void SubscribeToCommand(MethodInfo subscribeMethod, IBusClient busClient, object handler)
        {
            subscribeMethod.Invoke(busClient, new object[] 
            {
                new Func<ICommand, MessageContext, Task>((msg, ctx) 
                    => (handler as ICommandHandler<ICommand>).HandleAsync(msg))
            });
        }

        static void SubscribeToEvent(MethodInfo subscribeMethod, IBusClient busClient, object handler)
        {
            subscribeMethod.Invoke(busClient, new object[] 
            {
                new Func<IEvent, MessageContext, Task>((msg, ctx) 
                    => (handler as IEventHandler<IEvent>).HandleAsync(msg))
            });
        }
    }
}