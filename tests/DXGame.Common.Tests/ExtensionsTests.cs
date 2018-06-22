using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Moq;
using DXGame.Common.Communication;
using DXGame.Common.Communication.Extensions;
using DXGame.Messages.Commands.Playroom;
using System.Threading.Tasks;
using System.Threading;
using DXGame.Messages.Abstract;
using DXGame.Common.Hosting;

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
            var bus = new Mock<IMessageBus>();

            bus.Object.AddAssemblySubscribtions(webHost.Services);

            bus.Verify(b 
                => b.SubscribeAsync(It.IsAny<Func<StartGame, Task>>())
            );
        }

        [TestMethod]
        public void Test()
        {
            var webHost = new Mock<IWebHost>();
            var bus = new Mock<IMessageBus>();
            var eventSubscriber = new Mock<IEventSubscriber>();
            var builder = new ServiceHost.CommonSubscribtionBuilder(webHost.Object, bus.Object, eventSubscriber.Object);

            builder.SubscribeToAllDXGameEvents();

            eventSubscriber.Verify(es => es.OnEventReceived, Times.AtLeast(20));
        }
    }

    public class TestHandler : ICommandHandler<StartGame>
    {
        public Task HandleAsync(StartGame command) => Task.CompletedTask;
    }
}
