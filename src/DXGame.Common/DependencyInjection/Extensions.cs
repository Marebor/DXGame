using System.Linq;
using System.Reflection;
using Autofac;
using DXGame.Common.Helpers;
using DXGame.Messages.Commands;
using DXGame.Messages.Events;
using Microsoft.Extensions.DependencyInjection;

namespace DXGame.Common.DependencyInjection
{
    public static class Extensions
    {
        public static void AddDeclaredMessageHandlers(this IServiceCollection services)
        {
            ForEachMessageHandlerInAssembly.Execute(
                (handlerType, handlerInterface, messageType) => services.AddScoped(handlerInterface, handlerType),
                Assembly.GetCallingAssembly()
            );
        }
    }
}