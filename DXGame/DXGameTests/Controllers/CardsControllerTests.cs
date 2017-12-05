using Microsoft.VisualStudio.TestTools.UnitTesting;
using DXGame.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;
using DXGame.Models;
using DXGame.Models.Entities;

namespace DXGame.Controllers.Tests
{
    [TestClass()]
    public class CardsControllerTests
    {
        [TestMethod()]
        public void GetCardsTest()
        {
            var mock = PrepareMockRepo();
            var controller = new CardsController(mock.Object);

            var result = controller.GetCards();

            Assert.AreEqual(result.Count(), 3);
            Assert.AreEqual(result.First().ID, 1);
            Assert.AreEqual(result.Last().ID, 3);
        }
        
        [TestMethod()]
        public void DeleteCardTest()
        {
            Assert.Fail();
        }

        private Mock<ICardsRepository> PrepareMockRepo()
        {
            var mock = new Mock<ICardsRepository>();
            mock.Setup(m => m.Cards).Returns(new Card[]
            {
                new Card() { ID = 1, URL = "Content/Cards/Card_ID-0000000001.jpg" },
                new Card() { ID = 2, URL = "Content/Cards/Card_ID-0000000002.jpg" },
                new Card() { ID = 3, URL = "Content/Cards/Card_ID-0000000003.jpg" },
            });

            return mock;
        }
    }
}