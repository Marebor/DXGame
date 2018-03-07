using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Game
{
    public class GameFinished : IEvent
    {
        public Guid Playroom { get; }
        public Guid Game { get; }
        public int? AppliedOnAggregateVersion { get; }

        public GameFinished(Guid playroom, Guid game, int? appliedOnAggregateVersion)
        {
            this.Playroom = playroom;
            this.Game = game;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
        }
    }
}