using Microsoft.VisualStudio.TestTools.UnitTesting;
using DXGame.Controllers;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Net;
using System.Net.Http;

using Moq;
using Moq.Protected;
using Unity;
using DXGame.Models.Abstract;
using DXGame.Models.Entities;
using DXGame.Providers;
using DXGame.Providers.Abstract;

using Newtonsoft.Json;

namespace DXGameTests.UnitTests
{
    [TestClass]
    public class PlayroomsControllerTests
    {
        PlayroomsController _controller;

        [TestInitialize]
        public void Init()
        {
            _controller = new PlayroomsController(
                MyMocks.PlayroomsRepository.Object,
                MyMocks.PlayersRepository.Object,
                MyMocks.EventsRepository.Object,
                MyMocks.Broadcast.Object, 
                MyMocks.RequestPlayernameProvider.Object
            );
        }

        [TestMethod]
        public void CanGetAllPlayrooms()
        {
            var playrooms = _controller.Get();

            Assert.AreEqual(3, playrooms.Count());
            StringAssert.Contains(playrooms.First().Name, "Playroom1");
            StringAssert.Contains(playrooms.Last().Name, "Playroom3");
        }

        [TestMethod]
        public void CanGetExistingPlayroom()
        {
            var result = _controller.Get("Playroom1").Result as OkNegotiatedContentResult<Playroom>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(3, result.Content.Players.Count);
            StringAssert.Contains(result.Content.Name, "Playroom1");
        }

        [TestMethod]
        public void CannotGetNotExistingPlayroom()
        {
            var result = _controller.Get("PlayroomX").Result as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CanCreateNewPlayroom()
        {
            var result = _controller.Post("NewPlayroom").Result as CreatedNegotiatedContentResult<Playroom>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(0, result.Content.Players.Count);
            Assert.AreEqual("api/playrooms/NewPlayroom", result.Location.OriginalString);
        }

        [TestMethod]
        public void CannotCreateDuplicatePlayroom()
        {
            var result = _controller.Post("Playroom1").Result as ConflictResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CannotCreatePlayroomAnonymously()
        {
            var mockRequestPlayernameProvider = new Mock<IRequestPlayernameProvider>();
            mockRequestPlayernameProvider.Setup(m => m.GetPlayername()).Returns(() => { return null; });
            mockRequestPlayernameProvider.Setup(m => m.CannotRetrievePlayernameErrorMessage).Returns("No playername");
            var controller = new PlayroomsController(
                MyMocks.PlayroomsRepository.Object,
                MyMocks.PlayersRepository.Object,
                MyMocks.EventsRepository.Object,
                MyMocks.Broadcast.Object,
                mockRequestPlayernameProvider.Object
            );

            var result = controller.Post("NewPlayroom").Result as BadRequestErrorMessageResult;
            
            Assert.IsNotNull(result);
            Assert.AreEqual("No playername", result.Message);
        }

        [TestMethod]
        public void CanDeleteExistingPlayroom()
        {
            var result = _controller.Delete("Playroom3").Result as OkNegotiatedContentResult<Playroom>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual("Playroom3", result.Content.Name);
        }

        [TestMethod]
        public void CannotDeleteNotExistingPlayroom()
        {
            var result = _controller.Delete("PlayroomX").Result as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CannotDeletePlayroomAnonymously()
        {
            var mockRequestPlayernameProvider = new Mock<IRequestPlayernameProvider>();
            mockRequestPlayernameProvider.Setup(m => m.GetPlayername()).Returns(() => { return null; });
            mockRequestPlayernameProvider.Setup(m => m.CannotRetrievePlayernameErrorMessage).Returns("No playername");
            var controller = new PlayroomsController(
                MyMocks.PlayroomsRepository.Object,
                MyMocks.PlayersRepository.Object,
                MyMocks.EventsRepository.Object,
                MyMocks.Broadcast.Object,
                mockRequestPlayernameProvider.Object
            );

            var result = controller.Delete("Playroom1").Result as BadRequestErrorMessageResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("No playername", result.Message);
        }

        [TestMethod]
        public void CanJoinEmptyPlayroom()
        {
            var result = _controller.Join("Playroom3").Result as OkResult;
            
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CanJoinNonEmptyPlayroomWithNewPlayer()
        {
            var result = _controller.Join("Playroom2").Result as OkResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CannotJoinPlayroomWithExistingPlayer()
        {
            var result = _controller.Join("Playroom1").Result as ConflictResult;

            Assert.IsNotNull(result);
        }
    }
}