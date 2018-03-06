using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Events.Playroom
{
    public class GameFinishFailed : IEvent
    {
        public Guid Playroom { get; }
        public Guid Game { get; }
        public string ReasonCode { get; }
        public int? AppliedOnAggregateVersion { get; }

        public GameFinishFailed(Guid playroom, Guid game, string reasonCode, int? appliedOnAggregateVersion)
        {
            this.Playroom = playroom;
            this.Game = game;
            this.ReasonCode = reasonCode;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
        }
    }
}