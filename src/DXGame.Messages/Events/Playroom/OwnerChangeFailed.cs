using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Events.Playroom
{
    public class OwnerChangeFailed : IEvent
    {
        public Guid Playroom { get; }
        public string ReasonCode { get; }
        public int? AppliedOnAggregateVersion { get; }

        public OwnerChangeFailed(Guid playroom, string reasonCode, int? appliedOnAggregateVersion)
        {
            this.Playroom = playroom;
            this.ReasonCode = reasonCode;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
        }
    }
}