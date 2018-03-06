using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Events.Playroom
{
    public class GameFinishReceived : IEvent
    {
        public Guid Playroom { get; }
        public Guid Game { get; }
        public int? AppliedOnAggregateVersion { get; }

        public GameFinishReceived(Guid playroom, Guid game, int? appliedOnAggregateVersion)
        {
            this.Playroom = playroom;
            this.Game = game;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
        }
    }
}