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
using RawRabbit.DependencyInjection.ServiceCollection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RawRabbit.Serialization;

namespace DXGame.Common.Communication.RabbitMQ
{
    public static class Extensions
    {
        public static void AddRawRabbit(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new RabbitMQSettings();
            var section = configuration.GetSection("rabbitmq");
            section.Bind(settings);
            var client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions
            {
                ClientConfiguration = settings,
                DependencyInjection = ioc => ioc.AddSingleton<ISerializer, CustomSerializer>()
            });
            services.AddSingleton<IBusClient>(_ => client);
            // var rawRabbitOptions = new RawRabbitOptions();
            // rawRabbitOptions.ClientConfiguration = settings;
            // services.AddRawRabbit(rawRabbitOptions);
            services.AddScoped<IMessageBus, RawRabbitMessageBus>();
        }

        // public static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        // {
        //     var cfg = new RabbitMQSettings();
        //     var section = configuration.GetSection("rabbitmq");
        //     section.Bind(cfg);

        //     var factory = new ConnectionFactory() 
        //     {
        //         VirtualHost = cfg.VirtualHost,
        //         UserName = cfg.Username,
        //         Password = cfg.Password,
        //         Port = cfg.Port,
        //         HostName = cfg.Hostnames.FirstOrDefault() ?? string.Empty,
        //         AutomaticRecoveryEnabled = cfg.AutomaticRecovery,
        //         TopologyRecoveryEnabled = cfg.TopologyRecovery,
        //         NetworkRecoveryInterval = cfg.RecoveryInterval,
        //         //ClientProperties = provider.GetService<IClientPropertyProvider>().GetClientProperties(cfg),
        //         Ssl = cfg.Ssl    
        //     };
        //     var connection = factory.CreateConnection(cfg.Hostnames);
        //     var channel = connection.CreateModel();
        //     var consumer = new EventingBasicConsumer(channel); 

        //     services.AddSingleton<EventingBasicConsumer>(_ => consumer);
        //     services.AddSingleton<IModel>(_ => channel);
        //     services.AddScoped<IMessageBus, RabbitMQMessageBus>();
        // }
    }
}