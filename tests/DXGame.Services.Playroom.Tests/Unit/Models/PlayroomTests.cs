using System;
using System.Collections.Generic;
using System.Linq;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Common.Models;
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
                new PlayroomCreated(playroomId, "Test", false, owner, null),
                new PlayerJoined(playroomId, Guid.NewGuid()),
            };
            var playroom = Aggregate.Builder.Build<Domain.Models.Playroom>(events);

            Assert.AreEqual(playroomId, playroom.Id);
            Assert.AreEqual(2, playroom.Players.Count());
            Assert.AreEqual(0, playroom.RecentlyAppliedEvents.Count());
        }

        [TestMethod]
        public void Can_Retrieve_Aggregate_Changes() 
        {
            var playroom = Domain.Models.Playroom.Create(Guid.NewGuid(), "Test", false, Guid.NewGuid(), null);
            playroom.MarkRecentlyAppliedEventsAsConfirmed();

            playroom.AddPlayer(Guid.NewGuid(), null);
            playroom.AddPlayer(Guid.NewGuid(), null);
            playroom.NewGame(Guid.NewGuid());

            Assert.AreEqual(3, playroom.RecentlyAppliedEvents.Count());
            Assert.AreEqual(typeof(PlayerJoined), playroom.RecentlyAppliedEvents.First().GetType());
            Assert.AreEqual(typeof(GameStartRequested), playroom.RecentlyAppliedEvents.Last().GetType());
        }
    }
}
