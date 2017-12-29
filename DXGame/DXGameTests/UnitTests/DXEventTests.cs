using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DXGame.Models.Entities;
using Newtonsoft.Json;

namespace DXGameTests.UnitTests
{
    [TestClass]
    public class DXEventTests
    {
        [TestMethod]
        public void CanValidateProperJsonObject()
        {
            var content = JsonConvert.SerializeObject(new { id = 1, sender = "Player1", action = "PostCard", cardNo = 100 });

            var result = DXEvent.ValidateContent(content);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CannotValidateNotProperJsonObject()
        {
            var content = JsonConvert.SerializeObject(new { id = 1, sender = "Player1", action = "PostCard", cardNo = 100 }) + "}";

            var result = DXEvent.ValidateContent(content);

            Assert.IsFalse(result);
        }
    }
}
