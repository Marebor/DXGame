using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Game
{
    public class GameFinished : IAggregateAppliedEvent
    {
        public GameFinished(Guid playroom, Guid game, int appliedOnAggregateVersion)
        {
            this.Playroom = playroom;
            this.Game = game;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;

        }
        public Guid Playroom { get; protected set; }
        public Guid Game { get; protected set; }
        public int AppliedOnAggregateVersion { get; protected set; }
    }
}