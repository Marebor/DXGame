using System.Linq;
using System.Reflection;
using DXGame.Common.Communication;
using DXGame.Common.Communication.RabbitMQ;
using DXGame.Common.Helpers;
using DXGame.Common.Models;
using DXGame.Common.Persistence;
using DXGame.Common.Persistence.MongoDB;
using DXGame.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DXGame.Common.DependencyInjection
{
    public static class Extensions
    {
        public static void AddDefaultDXGameDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = Assembly.GetCallingAssembly();
            services.AddAssemblyMessageHandlers(assembly);
            services.AddRabbitMQ(configuration);
            services.AddMongoDB(configuration);
            services.AddScoped<IMessageBus, RabbitMQMessageBus>();
            services.AddScoped<IEventStore, MongoDBEventStore>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IHandler, Handler>();
        }
        public static void AddAssemblyMessageHandlers(this IServiceCollection services, Assembly assembly = null)
        {
            ForEachMessageHandlerInAssembly.Execute(
                (handlerType, handlerInterface, messageType) => services.AddScoped(handlerInterface, handlerType),
                assembly != null ? assembly : Assembly.GetCallingAssembly()
            );
        }
    }
}