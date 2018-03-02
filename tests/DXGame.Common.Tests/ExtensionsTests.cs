using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using RawRabbit;
using Moq;
using DXGame.Common.Communication.RabbitMQ;
using DXGame.Messages.Commands.Playroom;
using System.Threading.Tasks;
using DXGame.Messages.Commands;
using RawRabbit.Pipe;
using System.Threading;

namespace DXGame.Common.Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        [TestMethod]
        public void RabbitMQExtensions()
        {
            var webHost = BuildWebHost(new string[] {} );
            var busClient = new Mock<IBusClient>();

            busClient.Object.AddSubscriptionsForMessageHandlers(webHost.Services);

            busClient.Verify(bus 
                => SubscribeMessageExtension.SubscribeAsync(
                        It.IsAny<IBusClient>(),
                        It.IsAny<Func<StartGame, Task>>(), 
                        It.IsAny<Action<IPipeContext>>(),
                        default(CancellationToken)
                    )
            );
        }
    }

    public class TestHandler : ICommandHandler<StartGame>
    {
        public Task HandleAsync(StartGame command) => Task.CompletedTask;
    }
}
