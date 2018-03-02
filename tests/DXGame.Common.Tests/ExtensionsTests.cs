using Microsoft.VisualStudio.TestTools.UnitTesting;
using DXGame.Common.Communication.RabbitMQ;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

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
            
        }
    }
}
