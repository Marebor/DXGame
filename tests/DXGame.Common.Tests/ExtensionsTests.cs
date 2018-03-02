using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using RawRabbit;
using Moq;
using DXGame.Common.Communication.RabbitMQ;
using DXGame.Messages.Commands.Playroom;
using RawRabbit.Context;
using System.Threading.Tasks;
using DXGame.Messages.Commands;

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

            busClient.Object.AddRegisteredMessageHandlers(webHost.Services);

            busClient.Verify(bus 
                => bus.SubscribeAsync(It.IsAny<Func<StartGame, MessageContext, Task>>(), null)
            );
        }
    }

    public class TestHandler : ICommandHandler<StartGame>
    {
        public Task HandleAsync(StartGame command) => Task.CompletedTask;
    }
}
