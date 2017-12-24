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
            var players = new List<Player>
            {
                new Player() { Name = "Player1", Playrooms = new List<Playroom>() },
                new Player() { Name = "Player2", Playrooms = new List<Playroom>() },
                new Player() { Name = "Player3", Playrooms = new List<Playroom>() },
            };

            var playrooms = new List<Playroom>
            {
                new Playroom() { Name = "Playroom1_P1P2P3", Players = new List<Player>() },
                new Playroom() { Name = "Playroom2_P2P3", Players = new List<Player>() },
                new Playroom() { Name = "Playroom3_Empty", Players = new List<Player>() },
            };

            players.First(p => p.Name == "Player1").Playrooms.Add(playrooms.First(p => p.Name == "Playroom1_P1P2P3"));
            players.First(p => p.Name == "Player2").Playrooms.Add(playrooms.First(p => p.Name == "Playroom2_P2P3"));
            players.First(p => p.Name == "Player2").Playrooms.Add(playrooms.First(p => p.Name == "Playroom2_P2P3"));
            players.First(p => p.Name == "Player3").Playrooms.Add(playrooms.First(p => p.Name == "Playroom2_P2P3"));
            players.First(p => p.Name == "Player3").Playrooms.Add(playrooms.First(p => p.Name == "Playroom2_P2P3"));

            playrooms.First(p => p.Name == "Playroom1_P1P2P3").Players.Add(players.First(p => p.Name == "Player1"));
            playrooms.First(p => p.Name == "Playroom1_P1P2P3").Players.Add(players.First(p => p.Name == "Player2"));
            playrooms.First(p => p.Name == "Playroom1_P1P2P3").Players.Add(players.First(p => p.Name == "Player3"));
            playrooms.First(p => p.Name == "Playroom2_P2P3").Players.Add(players.First(p => p.Name == "Player2"));
            playrooms.First(p => p.Name == "Playroom2_P2P3").Players.Add(players.First(p => p.Name == "Player3"));

            _controller = new PlayroomsController(
                MyMocks.GetPlayroomsRepository(playrooms).Object,
                MyMocks.GetPlayersRepository(players).Object,
                MyMocks.GetEventsRepository().Object,
                MyMocks.GetBroadcast().Object, 
                MyMocks.GetRequestPlayernameProvider("Player1").Object
            );
        }

        [TestMethod]
        public void CanGetAllPlayrooms()
        {
            var playrooms = _controller.Get();

            Assert.AreEqual(3, playrooms.Count());
            Assert.AreEqual(playrooms.First().Name, "Playroom1_P1P2P3");
            Assert.AreEqual(playrooms.Last().Name, "Playroom3_Empty");
        }

        [TestMethod]
        public void CanGetExistingPlayroom()
        {
            var result = _controller.Get("Playroom1_P1P2P3").Result as OkNegotiatedContentResult<Playroom>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(3, result.Content.Players.Count);
            StringAssert.Contains(result.Content.Name, "Playroom1_P1P2P3");
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
            var result = _controller.Post("Playroom1_P1P2P3").Result as ConflictResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CannotCreatePlayroomAnonymously()
        {
            var controller = new PlayroomsController(
                MyMocks.GetPlayroomsRepository().Object,
                MyMocks.GetPlayersRepository().Object,
                MyMocks.GetEventsRepository().Object,
                MyMocks.GetBroadcast().Object,
                MyMocks.GetRequestPlayernameProvider(null).Object
            );

            var result = controller.Post("NewPlayroom").Result as BadRequestErrorMessageResult;
            
            Assert.IsNotNull(result);
            Assert.AreEqual("Wrong playername", result.Message);
        }

        [TestMethod]
        public void CannotCreatePlayroomWithNotExistingPlayerName()
        {
            var controller = new PlayroomsController(
                MyMocks.GetPlayroomsRepository().Object,
                MyMocks.GetPlayersRepository().Object,
                MyMocks.GetEventsRepository().Object,
                MyMocks.GetBroadcast().Object,
                MyMocks.GetRequestPlayernameProvider("SomePlayer").Object
            );

            var result = controller.Post("NewPlayroom").Result as BadRequestErrorMessageResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Player with name SomePlayer doesn't exist", result.Message);
        }

        [TestMethod]
        public void CanDeleteExistingPlayroom()
        {
            var result = _controller.Delete("Playroom3_Empty").Result as OkNegotiatedContentResult<Playroom>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual("Playroom3_Empty", result.Content.Name);
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
            var controller = new PlayroomsController(
                MyMocks.GetPlayroomsRepository().Object,
                MyMocks.GetPlayersRepository().Object,
                MyMocks.GetEventsRepository().Object,
                MyMocks.GetBroadcast().Object,
                MyMocks.GetRequestPlayernameProvider(null).Object
            );

            var result = controller.Delete("Playroom3_Empty").Result as BadRequestErrorMessageResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Wrong playername", result.Message);
        }

        [TestMethod]
        public void CanJoinEmptyPlayroom()
        {
            var result = _controller.Join("Playroom3_Empty").Result as OkResult;
            
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CanJoinNonEmptyPlayroomWithNewPlayer()
        {
            var result = _controller.Join("Playroom2_P2P3").Result as OkResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CannotJoinPlayroomWithExistingPlayer()
        {
            var result = _controller.Join("Playroom1_P1P2P3").Result as ConflictResult;

            Assert.IsNotNull(result);
        }
    }
}