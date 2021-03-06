using System;
using System.Linq;
using System.Reflection;
using Autofac;
using DXGame.Common.Communication;

namespace DXGame.Common.Helpers
{
    public static class ForEachMessageHandlerInAssembly
    {
        public static void Execute(Action<Type, Type, Type> action, Assembly assembly = null)
        {
            if (assembly == null)
                assembly = Assembly.GetCallingAssembly();
                
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
                handlerInterface = handlerInterface.GetGenericTypeDefinition().MakeGenericType(msgType);
                
                action(handlerType, handlerInterface, msgType);
            }
        }
    }
}