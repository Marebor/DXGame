using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DXGame.Common.Helpers;
using DXGame.Common.Communication;
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
    }
}