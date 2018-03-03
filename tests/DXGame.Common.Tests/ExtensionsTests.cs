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
    }

    public class TestHandler : ICommandHandler<StartGame>
    {
        public Task HandleAsync(StartGame command) => Task.CompletedTask;
    }
}
