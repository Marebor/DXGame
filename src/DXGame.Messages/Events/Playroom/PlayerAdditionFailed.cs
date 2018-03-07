using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayerAdditionFailed : IEvent
    {
        public Guid Playroom { get; }
        public Guid Player { get; }
        public string ReasonCode { get; }
        public int? AppliedOnAggregateVersion { get; }

        public PlayerAdditionFailed(Guid playroom, Guid player, string reasonCode, int? appliedOnAggregateVersion)
        {
            this.Playroom = playroom;
            this.Player = player;
            this.ReasonCode = reasonCode;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
        }
    }
}