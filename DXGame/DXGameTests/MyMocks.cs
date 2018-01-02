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
        static ICollection<Playroom> _defaultPlayrooms = new List<Playroom>();
        static ICollection<Player> _defaultPlayers = new List<Player>();
        static ICollection<DXEvent> _defaultEvents = new List<DXEvent>();

        public static Mock<IPlayroomsRepository> PlayroomsRepository { get; private set; }
        public static Mock<IPlayersRepository> PlayersRepository { get; private set; }
        public static Mock<IEventsRepository> EventsRepository { get; private set; }
        public static Mock<IBroadcast> Broadcast { get; private set; }
        public static Mock<IRequestPlayernameProvider> RequestPlayernameProvider { get; private set; }
        public static Mock<IPlayroomsRepository> PreparePlayroomsRepository(ICollection<Playroom> playrooms = null)
        {
            if (playrooms == null) playrooms = _defaultPlayrooms;

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
            PlayroomsRepository = mock;

            return mock;
        }
        public static Mock<IPlayersRepository> PreparePlayersRepository(ICollection<Player> players = null)
        {
            if (players == null) players = _defaultPlayers;

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
            PlayersRepository = mock;

            return mock;
        }
        public static Mock<IEventsRepository> PrepareEventsRepository(ICollection<DXEvent> events = null)
        {
            if (events == null) events = _defaultEvents;

            var mock = new Mock<IEventsRepository>();
            mock.Setup(m => m.Events).Returns(events);
            mock.Setup(m => m.AddAsync(It.IsAny<DXEvent>())).Returns(async (DXEvent dxEvent) =>
            {
                await Task.Yield();
                if (dxEvent.ID != 0 && events.FirstOrDefault(e => e.ID == dxEvent.ID) != null) return null;
                if (dxEvent.ID == 0) dxEvent.ID = events.Count > 0 ? events.Select(e => e.ID).Max() + 1 : 1;
                events.Add(dxEvent);

                return dxEvent;
            });
            EventsRepository = mock;

            return mock;
        }
        public static Mock<IBroadcast> PrepareBroadcast()
        {
            //if (_broadcast != null) return _broadcast;
            var mock = new Mock<IBroadcast>();
            Broadcast = mock;

            return mock;
        }
        public static Mock<IRequestPlayernameProvider> PrepareRequestPlayernameProvider(string returningPlayername)
        {
            //if (_playernameProvider != null) return _playernameProvider;

            var mock = new Mock<IRequestPlayernameProvider>();
            mock.Setup(m => m.GetPlayername()).Returns(returningPlayername);
            mock.Setup(m => m.CannotRetrievePlayernameErrorMessage).Returns("Wrong playername");
            RequestPlayernameProvider = mock;

            return mock;
        }
    }
}
