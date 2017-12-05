using Microsoft.VisualStudio.TestTools.UnitTesting;
using DXGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;
using System.Web;
using System.Web.Http;
using DXGame.Models.Entities;

namespace DXGame.Models.Tests
{
    [TestClass()]
    public class FolderCardsRepositoryTests
    {
        [TestMethod()]
        public void AddAsync_CanAddFileWithNewID()
        {
            var mock = PrepareMock();
            var file = PrepareValidFile();

            var result = mock.Object.AddAsync(file.Object).Result;
        }

        [TestMethod()]
        public void DeleteAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FindAsyncTest()
        {
            Assert.Fail();
        }

        private Mock<FolderCardsRepository> PrepareMock()
        {
            var mock = new Mock<FolderCardsRepository>();
            mock.Setup(m => m.Cards).Returns(new Card[]
            {
                new Card() { ID = 1, URL = "Content/Cards/1" },
                new Card() { ID = 2, URL = "Content/Cards/2" },
                new Card() { ID = 3, URL = "Content/Cards/3" }
            });

            return mock;
        }

        private Mock<HttpPostedFileBase> PrepareValidFile()
        {
            var file = new Mock<HttpPostedFileBase>();
            file.Setup(f => f.FileName).Returns("testFile.jpg");

            return file;
        }
    }
}