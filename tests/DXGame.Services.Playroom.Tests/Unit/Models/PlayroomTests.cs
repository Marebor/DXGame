using System;
using System.Collections.Generic;
using System.Linq;
using DXGame.Common.Helpers;
using DXGame.Common.Models;
using DXGame.Messages.Abstract;
using DXGame.Messages.Commands.Playroom;
using DXGame.Messages.Events.Playroom;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DXGame.Services.Playroom.Tests
{
    [TestClass]
    public class PlayroomTests
    {
        [TestMethod]
        public void Can_build_playroom()
        {
            var playroomId = Guid.NewGuid();
            var owner = Guid.NewGuid();
            var events = new List<IEvent> 
            {
                new PlayroomCreationRequested(playroomId, "Test", false, owner, null, 0, Guid.NewGuid()),
                new PlayroomCreated(playroomId, "Test", false, owner, 1, Guid.NewGuid()),
                new PlayerJoined(playroomId, Guid.NewGuid(), 2, Guid.NewGuid()),
            };
            var playroom = Aggregate.Builder.Build<Domain.Models.Playroom>(events);

            Assert.AreEqual(playroomId, playroom.Id);
            Assert.AreEqual(2, playroom.Players.Count());
            Assert.AreEqual(0, playroom.RecentlyAppliedEvents.Count());
            Assert.AreEqual(3, playroom.Version);
        }

        [TestMethod]
        public void Can_Retrieve_Aggregate_Changes() 
        {
            var command = new CreatePlayroom(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test",
                Guid.NewGuid(),
                false,
                null
            );
            var playroom = Domain.Models.Playroom.Create(command);
            playroom.MarkRecentlyAppliedEventsAsConfirmed();

            playroom.AddPlayer(new AddPlayer(Guid.NewGuid(),Guid.NewGuid(), Guid.NewGuid(), null));
            playroom.AddPlayer(new AddPlayer(Guid.NewGuid(),Guid.NewGuid(), Guid.NewGuid(), null));
            playroom.NewGame(new StartGame(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), playroom.Players.First()));

            Assert.AreEqual(3, playroom.RecentlyAppliedEvents.Count());
            Assert.AreEqual(typeof(PlayerJoined), playroom.RecentlyAppliedEvents.First().GetType());
            Assert.AreEqual(typeof(GameStartRequested), playroom.RecentlyAppliedEvents.Last().GetType());
        }
    }
}
