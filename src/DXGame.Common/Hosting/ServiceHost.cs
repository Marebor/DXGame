using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using DXGame.Common.Communication;
using DXGame.Common.Communication.Extensions;
using DXGame.Common.Communication.RabbitMQ;
using System.Reflection;
using DXGame.Messages.Abstract;
using System.Threading.Tasks;
using System.Linq;
using Autofac;

namespace DXGame.Common.Hosting
{
    public class ServiceHost
    {
        private readonly IWebHost _webHost;

        public ServiceHost(IWebHost webHost)
        {
            _webHost = webHost;
        }

        public void Run() => _webHost.Run();

        public static HostBuilder Create<TStartup>(string[] args) where TStartup : class
        {
            Console.Title = typeof(TStartup).Namespace;
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseStartup<TStartup>()
                .UseDefaultServiceProvider(options => options.ValidateScopes = false);

            return new HostBuilder(webHostBuilder.Build());
        }

        public abstract class BuilderBase 
        {
            public abstract ServiceHost Build();
        }

        public class HostBuilder : BuilderBase
        {
            private readonly IWebHost _webHost;
            private IMessageBus _bus;

            public HostBuilder(IWebHost webHost)
            {
                _webHost = webHost;
            }

            public BusBuilder UseMessageBus()
            {
                _bus = (IMessageBus)_webHost.Services.GetService(typeof(IMessageBus));

                return new BusBuilder(_webHost, _bus);
            }

            public override ServiceHost Build()
            {
                return new ServiceHost(_webHost);
            }
        }

        public class BusBuilder : BuilderBase
        {
            private readonly IWebHost _webHost;
            private IMessageBus _bus; 

            public BusBuilder(IWebHost webHost, IMessageBus bus)
            {
                _webHost = webHost;
                _bus = bus;
            }

            public BusBuilder AddAssemblySubscriptions() 
            {
                _bus.AddAssemblySubscribtions(_webHost.Services, Assembly.GetEntryAssembly());

                return this;
            }

            public CommonSubscribtionBuilder UseCommonEventSubscriber()
            {
                var subscriber = _webHost.Services.GetService(typeof(IEventSubscriber)) as IEventSubscriber;

                return new CommonSubscribtionBuilder(_webHost, _bus, subscriber);
            }

            public override ServiceHost Build()
            {
                return new ServiceHost(_webHost);
            }
        }

        public class CommonSubscribtionBuilder : BuilderBase
        {
            IWebHost _webHost;
            IMessageBus _bus;
            IEventSubscriber _subscriber;

            public CommonSubscribtionBuilder(IWebHost webHost, IMessageBus bus, IEventSubscriber subscriber)
            {
                _webHost = webHost;
                _bus = bus;
                _subscriber = subscriber;
            }

            public CommonSubscribtionBuilder SubscribeToAllDXGameEvents()
            {
                var messagesAssembly = typeof(IEvent).Assembly;           
                    
                var events = messagesAssembly
                    .GetTypes()
                    .Where(t => t.IsAssignableTo<IEvent>());

                foreach (var e in events)
                {
                    DXGame.Common.Communication.Extensions.Extensions
                        .MessageBusSubscriptionMethod(e).Invoke(_bus, new object[] { _subscriber.OnEventReceived });
                }

                return this;
            }

            public override ServiceHost Build()
            {
                return new ServiceHost(_webHost);
            }
        }
    }
}