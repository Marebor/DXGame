using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class GameFinishReceived : IAggregateAppliedEvent
    {
        public GameFinishReceived(Guid playroom, Guid game, int appliedOnAggregateVersion)
        {
            this.Playroom = playroom;
            this.Game = game;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;

        }
        public Guid Playroom { get; }
        public Guid Game { get; }
        public int AppliedOnAggregateVersion { get; }
    }
}