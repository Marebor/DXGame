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
    [TestClass()]
    public class PlayroomsControllerTests
    {
        ApiController _controller;

        [TestInitialize]
        void Init()
        {
            var mockPlayroomsRepo = PreparePlayroomsRepository();
        }

        Mock<IPlayroomsRepository> PreparePlayroomsRepository()
        {
            var players = new List<Player>()
            {
                new Player() { Name = "Player1" },
                new Player() { Name = "Player2" },
                new Player() { Name = "Player3" },
            };
            var playrooms = new List<Playroom>()
            {
                new Playroom() { Name = "Playroom1", Players = players },
                new Playroom() { Name = "Playroom2", Players = new List<Player>() },
                new Playroom() { Name = "Playroom3", Players = new List<Player>() },
            };
            var mock = new Mock<IPlayroomsRepository>();
            mock.Setup(m => m.Playrooms).Returns(playrooms);
            mock.Setup(m => m.AddAsync(It.IsAny<Playroom>())).Returns(async (Playroom playroom) => 
            {
                if (playroom == null || await mock.Object.FindAsync(playroom.Name) != null) return null;
                (mock.Object.Playrooms as List<Playroom>).Add(playroom);

                return playroom;
            });
            mock.Setup(m => m.DeleteAsync(It.IsAny<string>())).Returns(async (string name) =>
            {
                var playroom = await mock.Object.FindAsync(name);
                if (playroom != null)
                {
                    (mock.Object.Playrooms as List<Playroom>).Remove(playroom);
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

            return mock;
        }

        Mock<IPlayersRepository> PreparePlayersRepository()
        {

        }
    }
}