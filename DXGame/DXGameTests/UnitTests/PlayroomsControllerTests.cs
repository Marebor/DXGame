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
                new Player() { Name = "Player1" },
                new Player() { Name = "Player2" },
                new Player() { Name = "Player3" },
            };

            var playrooms = new List<Playroom>
            {
                new Playroom() { Name = "Playroom1_P1P2P3", Players = new List<Player>() },
                new Playroom() { Name = "Playroom2_P2P3", Players = new List<Player>() },
                new Playroom() { Name = "Playroom3_Empty", Players = new List<Player>() },
            };        

            //players.First(p => p.Name == "Player1").Playrooms.Add(playrooms.First(p => p.Name == "Playroom1_P1P2P3").Name);
            //players.First(p => p.Name == "Player2").Playrooms.Add(playrooms.First(p => p.Name == "Playroom2_P2P3").Name);
            //players.First(p => p.Name == "Player2").Playrooms.Add(playrooms.First(p => p.Name == "Playroom2_P2P3").Name);
            //players.First(p => p.Name == "Player3").Playrooms.Add(playrooms.First(p => p.Name == "Playroom2_P2P3").Name);
            //players.First(p => p.Name == "Player3").Playrooms.Add(playrooms.First(p => p.Name == "Playroom2_P2P3").Name);

            playrooms.First(p => p.Name == "Playroom1_P1P2P3").Players.Add(players.First(p => p.Name == "Player1"));
            playrooms.First(p => p.Name == "Playroom1_P1P2P3").Players.Add(players.First(p => p.Name == "Player2"));
            playrooms.First(p => p.Name == "Playroom1_P1P2P3").Players.Add(players.First(p => p.Name == "Player3"));
            playrooms.First(p => p.Name == "Playroom2_P2P3").Players.Add(players.First(p => p.Name == "Player2"));
            playrooms.First(p => p.Name == "Playroom2_P2P3").Players.Add(players.First(p => p.Name == "Player3"));

            var events = new List<DXEvent>
            {
                new DXEvent("Playroom1 Created") { DatePerformed = DateTime.Now.AddMinutes(-20), PerformedBy = "Player1", PlayroomName = "Playroom1_P1P2P3" },
                new DXEvent("Playroom2 Created") { DatePerformed = DateTime.Now.AddMinutes(-19), PerformedBy = "Player1", PlayroomName = "Playroom2_P2P3" },
                new DXEvent("Playroom3 Created") { DatePerformed = DateTime.Now.AddMinutes(-18), PerformedBy = "Player1", PlayroomName = "Playroom3_Empty" },
                new DXEvent("Player1 joined Playroom1") { DatePerformed = DateTime.Now.AddMinutes(-17), PerformedBy = "Player1", PlayroomName = "Playroom1_P1P2P3" },
                new DXEvent("Player2 joined Playroom1") { DatePerformed = DateTime.Now.AddMinutes(-16), PerformedBy = "Player2", PlayroomName = "Playroom1_P1P2P3" },
                new DXEvent("Player3 joined Playroom1") { DatePerformed = DateTime.Now.AddMinutes(-15), PerformedBy = "Player3", PlayroomName = "Playroom1_P1P2P3" },
                new DXEvent("Player2 joined Playroom2") { DatePerformed = DateTime.Now.AddMinutes(-14), PerformedBy = "Player2", PlayroomName = "Playroom2_P2P3" },
                new DXEvent("Player3 joined Playroom2") { DatePerformed = DateTime.Now.AddMinutes(-13), PerformedBy = "Player3", PlayroomName = "Playroom2_P2P3" },
            };

            _controller = new PlayroomsController(
                MyMocks.PreparePlayroomsRepository(playrooms).Object,
                MyMocks.PreparePlayersRepository(players).Object,
                MyMocks.PrepareEventsRepository(events).Object,
                MyMocks.PrepareBroadcast().Object, 
                MyMocks.PrepareRequestPlayernameProvider("Player1").Object
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
            Assert.AreEqual("api/playrooms/newplayroom", result.Location.OriginalString);
            MyMocks.PlayroomsRepository.Verify(m => m.AddAsync(It.Is<Playroom>(p => p.Name == "NewPlayroom")));
            MyMocks.EventsRepository.Verify(m => m.AddAsync(It.Is<DXEvent>(e => e.PerformedBy == "Player1" && e.PlayroomName == "NewPlayroom")));
            MyMocks.Broadcast.Verify(m => m.Broadcast(It.IsAny<string>()));
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
                MyMocks.PlayroomsRepository.Object,
                MyMocks.PlayersRepository.Object,
                MyMocks.EventsRepository.Object,
                MyMocks.Broadcast.Object,
                MyMocks.PrepareRequestPlayernameProvider(null).Object
            );

            var result = controller.Post("NewPlayroom").Result as BadRequestErrorMessageResult;
            
            Assert.IsNotNull(result);
            Assert.AreEqual("Wrong playername", result.Message);
        }

        [TestMethod]
        public void CannotCreatePlayroomWithNotExistingPlayerName()
        {
            var controller = new PlayroomsController(
                MyMocks.PlayroomsRepository.Object,
                MyMocks.PlayersRepository.Object,
                MyMocks.EventsRepository.Object,
                MyMocks.Broadcast.Object,
                MyMocks.PrepareRequestPlayernameProvider("SomePlayer").Object
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
            MyMocks.PlayroomsRepository.Verify(m => m.DeleteAsync(It.Is<string>(s => s == "Playroom3_Empty"))); 
            MyMocks.EventsRepository.Verify(m => m.AddAsync(It.Is<DXEvent>(e => e.PerformedBy == "Player1" && e.PlayroomName == "Playroom3_Empty")));
            MyMocks.Broadcast.Verify(m => m.Broadcast(It.IsAny<string>()));
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
                MyMocks.PlayroomsRepository.Object,
                MyMocks.PlayersRepository.Object,
                MyMocks.EventsRepository.Object,
                MyMocks.Broadcast.Object,
                MyMocks.PrepareRequestPlayernameProvider(null).Object
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
            MyMocks.PlayroomsRepository.Verify(m => m.AddPlayerToPlayroomAsync(It.Is<Player>(p => p.Name == "Player1"), It.Is<string>(s => s == "Playroom3_Empty")));
            MyMocks.EventsRepository.Verify(m => m.AddAsync(It.Is<DXEvent>(e => e.PerformedBy == "Player1" && e.PlayroomName == "Playroom3_Empty")));
            MyMocks.Broadcast.Verify(m => m.Broadcast(It.IsAny<string>()));
        }

        [TestMethod]
        public void CanJoinNonEmptyPlayroomWithNewPlayer()
        {
            var result = _controller.Join("Playroom2_P2P3").Result as OkResult;

            Assert.IsNotNull(result);
            MyMocks.PlayroomsRepository.Verify(m => m.AddPlayerToPlayroomAsync(It.Is<Player>(p => p.Name == "Player1"), It.Is<string>(s => s == "Playroom2_P2P3")));
            MyMocks.EventsRepository.Verify(m => m.AddAsync(It.Is<DXEvent>(e => e.PerformedBy == "Player1" && e.PlayroomName == "Playroom2_P2P3")));
            MyMocks.Broadcast.Verify(m => m.Broadcast(It.IsAny<string>()));
        }

        [TestMethod]
        public void CannotJoinPlayroomWithExistingPlayer()
        {
            var result = _controller.Join("Playroom1_P1P2P3").Result as ConflictResult;

            Assert.IsNotNull(result);
        }

        [TestMethod] 
        public void CannotJoinPlayroomAnonymously()
        {
            var controller = new PlayroomsController(
                MyMocks.PlayroomsRepository.Object,
                MyMocks.PlayersRepository.Object,
                MyMocks.EventsRepository.Object,
                MyMocks.Broadcast.Object,
                MyMocks.PrepareRequestPlayernameProvider(null).Object
            );

            var result = controller.Join("Playroom3_Empty").Result as BadRequestErrorMessageResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Wrong playername", result.Message);
        }

        [TestMethod]
        public void CannotLeaveEmptyPlayroom()
        {
            var result = _controller.Leave("Playroom3_Empty").Result as BadRequestErrorMessageResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CannotLeaveNonEmptyPlayroomWithNewPlayer()
        {
            var result = _controller.Leave("Playroom2_P2P3").Result as BadRequestErrorMessageResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CanLeavePlayroomWithExistingPlayer()
        {
            var result = _controller.Leave("Playroom1_P1P2P3").Result as OkResult;

            Assert.IsNotNull(result);
            MyMocks.PlayroomsRepository.Verify(m => m.RemovePlayerFromPlayroomAsync(It.Is<string>(s => s == "Player1"), It.Is<string>(s => s == "Playroom1_P1P2P3")));
            MyMocks.EventsRepository.Verify(m => m.AddAsync(It.Is<DXEvent>(e => e.PerformedBy == "Player1" && e.PlayroomName == "Playroom1_P1P2P3")));
            MyMocks.Broadcast.Verify(m => m.Broadcast(It.IsAny<string>()));
        }

        [TestMethod]
        public void CannotLeavePlayroomAnonymously()
        {
            var controller = new PlayroomsController(
                MyMocks.PlayroomsRepository.Object,
                MyMocks.PlayersRepository.Object,
                MyMocks.EventsRepository.Object,
                MyMocks.Broadcast.Object,
                MyMocks.PrepareRequestPlayernameProvider(null).Object
            );
            
            var result = controller.Leave("Playroom3_Empty").Result as BadRequestErrorMessageResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Wrong playername", result.Message);
        }

        [TestMethod]
        public void CanGetAllEventsRelatedToExistingPlayroom()
        {
            var result = _controller.GetEvents("Playroom1_P1P2P3").Result as OkNegotiatedContentResult<IEnumerable<string>>;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Count());
        }

        [TestMethod]
        public void CanGetAllEventsRelatedToExistingPlayroomAnonymously()
        {
            var controller = new PlayroomsController(
                MyMocks.PlayroomsRepository.Object,
                MyMocks.PlayersRepository.Object,
                MyMocks.EventsRepository.Object,
                MyMocks.Broadcast.Object,
                MyMocks.PrepareRequestPlayernameProvider(null).Object
            );

            var result = controller.GetEvents("Playroom1_P1P2P3").Result as OkNegotiatedContentResult<IEnumerable<string>>;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Count());
        }

        [TestMethod]
        public void CannotGetEventsRelatedToNotExistingPlayroom()
        {
            var result = _controller.GetEvents("PlayroomX").Result as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CanPostNewEventToExistingPlayroom()
        {
            var eventContent = JsonConvert.SerializeObject(new { Description = "Some event" });
            var result = _controller.PostEvent("Playroom1_P1P2P3", eventContent).Result as OkResult;

            Assert.IsNotNull(result);
            MyMocks.EventsRepository.Verify(
                m => m.AddAsync(It.Is<DXEvent>(e => e.Content == eventContent && e.PerformedBy == "Player1" && e.PlayroomName == "Playroom1_P1P2P3")));
            MyMocks.Broadcast.Verify(m => m.BroadcastGroup(It.Is<string>(s => s == eventContent), It.Is<string>(s => s == "Playroom1_P1P2P3")));
        }

        [TestMethod]
        public void CannotPostEventToExistingPlayroomAnonymously()
        {
            MyMocks.PrepareRequestPlayernameProvider(null);

            var controller = new PlayroomsController(
                MyMocks.PlayroomsRepository.Object,
                MyMocks.PlayersRepository.Object,
                MyMocks.EventsRepository.Object,
                MyMocks.Broadcast.Object,
                MyMocks.RequestPlayernameProvider.Object
            );

            var result = controller.PostEvent("Playroom1_P1P2P3", JsonConvert.SerializeObject(new { Description = "Some event" })).Result as BadRequestErrorMessageResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CannotPostEventToNotExistingPlayroom()
        {
            var result = _controller.PostEvent("PlayroomX", JsonConvert.SerializeObject(new { Description = "Some event" })).Result as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CannotPostEventToExistingPlayroomNotBeingPartOfIt()
        {
            var result = _controller.PostEvent("Playroom2_P2P3", JsonConvert.SerializeObject(new { Description = "Some event" })).Result as BadRequestResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CannotPostNotProperEvent()
        {
            var result = _controller.PostEvent("Playroom2_P2P3", "Some event").Result as BadRequestErrorMessageResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Posted event is not an object", result.Message);
        }
    }
}