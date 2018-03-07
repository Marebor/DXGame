using System;
using System.Collections.Generic;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class GameStartRequested : IEvent
    {
        public Guid Playroom { get; }
        public Guid Game { get; }
        public IEnumerable<Guid> Players { get; }
        public int? AppliedOnAggregateVersion { get; }

        public GameStartRequested(Guid playroom, Guid game, int? appliedOnAggregateVersion)
        {
            this.Playroom = playroom;
            this.Game = game;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
        }
    }
}