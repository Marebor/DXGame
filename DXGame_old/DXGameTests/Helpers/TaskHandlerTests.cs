using Microsoft.VisualStudio.TestTools.UnitTesting;
using DXGame.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXGame.Helpers.Tests
{
    [TestClass()]
    public class TaskHandlerTests
    {
        [TestMethod()]
        public void ExecuteAsyncTest()
        {
            var handler = new TaskHandler();

            handler
                .Run(async () =>
                {
                    await Task.CompletedTask;
                    throw new StackOverflowException();
                })
                .OnSuccess(async () =>
                {
                    await Task.CompletedTask;
                })
                .OnCustomError<ArgumentException>(async (ex) =>
                {
                    await Task.CompletedTask;
                })
                .OnCustomError<ArgumentNullException>(async (ex) =>
                {
                    await Task.CompletedTask;
                })
                .OnError(async (ex) =>
                {
                    await Task.CompletedTask;
                })
                .Finally(async () =>
                {
                    await Task.CompletedTask;
                })
                .DoNotPropagateException()
                .ExecuteAsync().Wait();
        }
    }
}