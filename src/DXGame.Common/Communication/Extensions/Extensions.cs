using System;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using DXGame.Common.Helpers;
using DXGame.Messages.Abstract;

namespace DXGame.Common.Communication.Extensions
{
    public static class Extensions
    {
        public static void AddAssemblySubscribtions(this IMessageBus bus, 
            IServiceProvider serviceProvider, Assembly assembly = null)
        {
            ForEachMessageHandlerInAssembly.Execute(
                (handlerType, handlerInterface, messageType) =>
                {
                    var handler = serviceProvider.GetService(handlerInterface);
                    bus.SubscribeToMessage(messageType, handler);
                },
                assembly ?? Assembly.GetEntryAssembly()
            );
        }

        public static void SubscribeToMessage(this IMessageBus bus, Type msgType, object handler)
        {
            var subscribeMethod = MessageBusSubscriptionMethod(msgType);
            subscribeMethod.Invoke(bus, new object[] { handler });
        }

        public static MethodInfo MessageBusSubscriptionMethod(Type genericType)
            => typeof(IMessageBus)
                .GetMethod(nameof(IMessageBus.SubscribeAsync))
                .MakeGenericMethod(genericType);

        static Func<ICommand, Task> CommandHandlerHandleAsyncMethod(object handler)
            => msg => (handler as ICommandHandler<ICommand>).HandleAsync(msg);
        
        static Func<IEvent, Task> EventHandlerHandleAsyncMethod(object handler)
            => msg => (handler as IEventHandler<IEvent>).HandleAsync(msg);
    }
}