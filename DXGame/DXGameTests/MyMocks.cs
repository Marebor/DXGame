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

namespace DXGameTests
{
    public static class MyMocks
    {
        static Mock<IPlayroomsRepository> _playroomsRepository;
        static Mock<IPlayersRepository> _playersRepository;
        static Mock<IEventsRepository> _eventsRepository;
        static Mock<IBroadcast> _broadcast;
        static Mock<IRequestPlayernameProvider> _playernameProvider;
        public static Mock<IPlayroomsRepository> GetPlayroomsRepository(ICollection<Playroom> playrooms = new List<Playroom>)
        {
            //var players1 = new List<Player>()
            //    {
            //        new Player() { Name = "Player1" },
            //        new Player() { Name = "Player2" },
            //        new Player() { Name = "Player3" },
            //    };
            //var players2 = new List<Player>()
            //    {
            //        new Player() { Name = "Player1" },
            //        new Player() { Name = "Player2" },
            //    };
            //var playrooms = new List<Playroom>()
            //    {
            //        new Playroom() { Name = "Playroom1", Players = players1 },
            //        new Playroom() { Name = "Playroom2", Players = players2 },
            //        new Playroom() { Name = "Playroom3", Players = new List<Player>() },
            //    };
            if (_playroomsRepository != null) return _playroomsRepository;

            var mock = new Mock<IPlayroomsRepository>();
            mock.Setup(m => m.Playrooms).Returns(playrooms);
            mock.Setup(m => m.AddAsync(It.IsAny<Playroom>())).Returns(async (Playroom playroom) =>
            {
                if (playroom == null || await mock.Object.FindAsync(playroom.Name) != null) return null;
                playrooms.Add(playroom);

                return playroom;
            });
            mock.Setup(m => m.DeleteAsync(It.IsAny<string>())).Returns(async (string name) =>
            {
                var playroom = await mock.Object.FindAsync(name);
                if (playroom != null)
                {
                    playrooms.Remove(playroom);
                }

                return playroom;
            });
            mock.Setup(m => m.FindAsync(It.IsAny<string>())).Returns(async (string name) =>
            {
                await Task.Yield();
                return mock.Object.Playrooms.FirstOrDefault(p => p.Name == name);
            });
            mock.Setup(m => m.AddPlayerToPlayroomAsync(It.IsAny<Player>(), It.IsAny<string>())).Returns(async (Player player, string playroomName) =>
            {
                var playroom = await mock.Object.FindAsync(playroomName);
                if (player == null || playroom == null || playroom.Players.FirstOrDefault(p => p.Name == player.Name) != null) return null;

                playroom.Players.Add(player);

                return playroom;
            });
            mock.Setup(m => m.RemovePlayerFromPlayroomAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(async (string playerName, string playroomName) =>
            {
                var playroom = await mock.Object.FindAsync(playroomName);
                if (playroom == null) return null;
                var player = playroom.Players.FirstOrDefault(p => p.Name == playerName);
                if (player == null) return null;

                playroom.Players.Remove(player);

                return playroom;
            });
            _playroomsRepository = mock;

            return mock;
        }
        public static Mock<IPlayersRepository> GetPlayersRepository(ICollection<Player> players = new List<Player>)
        {
            //var playrooms = new List<Playroom>() { new Playroom() { Name = "Playroom1" } };
            //var players = new List<Player>()
            //    {
            //        new Player() { Name = "Player1", Playrooms = playrooms },
            //        new Player() { Name = "Player2", Playrooms = playrooms },
            //        new Player() { Name = "Player3", Playrooms = playrooms },
            //    };
            if (_playersRepository != null) return _playersRepository;
            var mock = new Mock<IPlayersRepository>();
            mock.Setup(m => m.Players).Returns(players);
            mock.Setup(m => m.AddAsync(It.IsAny<Player>())).Returns(async (Player player) =>
            {
                if (player == null || await mock.Object.FindAsync(player.Name) != null) return null;
                players.Add(player);

                return player;
            });
            mock.Setup(m => m.DeleteAsync(It.IsAny<string>())).Returns(async (string name) =>
            {
                var player = await mock.Object.FindAsync(name);
                if (player != null)
                {
                    players.Remove(player);
                }

                return player;
            });
            mock.Setup(m => m.FindAsync(It.IsAny<string>())).Returns(async (string name) =>
            {
                await Task.Yield();
                return mock.Object.Players.FirstOrDefault(p => p.Name == name);
            });
            _playersRepository = mock;

            return mock;
        }
        public static Mock<IEventsRepository> GetEventsRepository(ICollection<DXEvent> events = new List<DXEvent>)
        {
            //var events = new List<DXEvent>()
            //    {
            //        new DXEvent() { ID = 1, PerformedBy = "Player1", DatePerformed = DateTime.Now.AddMinutes(-3), PlayroomName = "Playroom1", Content = "Content1" },
            //        new DXEvent() { ID = 2, PerformedBy = "Player2", DatePerformed = DateTime.Now.AddMinutes(-2), PlayroomName = "Playroom1", Content = "Content2" },
            //        new DXEvent() { ID = 3, PerformedBy = "Player1", DatePerformed = DateTime.Now.AddMinutes(-1), PlayroomName = "Playroom1", Content = "Content3" },
            //    };
            var mock = new Mock<IEventsRepository>();
            mock.Setup(m => m.Events).Returns(events);
            mock.Setup(m => m.AddAsync(It.IsAny<DXEvent>())).Returns(async (DXEvent dxEvent) =>
            {
                await Task.Yield();
                if (dxEvent.ID != 0 && events.FirstOrDefault(e => e.ID == dxEvent.ID) != null) return null;
                if (dxEvent.ID == 0) dxEvent.ID = events.Select(e => e.ID).Max() + 1;
                events.Add(dxEvent);

                return dxEvent;
            });

            return mock;
        }
        public static Mock<IBroadcast> Broadcast
        {
            get
            {
                return new Mock<IBroadcast>();
            }
        }
        public static Mock<IRequestPlayernameProvider> RequestPlayernameProvider
        {
            get
            {
                var mock = new Mock<IRequestPlayernameProvider>();
                mock.Setup(m => m.GetPlayername()).Returns("Player3");

                return mock;
            }
        }
    }
}
