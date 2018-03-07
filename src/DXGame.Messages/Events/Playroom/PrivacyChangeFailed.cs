using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PrivacyChangeFailed : IEvent
    {
        public Guid Playroom { get; }
        public string ReasonCode { get; }
        public int? AppliedOnAggregateVersion { get; }

        public PrivacyChangeFailed(Guid playroom, string reasonCode, int? appliedOnAggregateVersion)
        {
            this.Playroom = playroom;
            this.ReasonCode = reasonCode;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
        }
    }
}