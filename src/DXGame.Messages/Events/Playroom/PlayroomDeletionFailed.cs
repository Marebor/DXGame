using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayroomDeletionFailed : IEvent
    {
        public Guid PlayroomId { get; }
        public string ReasonCode { get; }
        public int? AppliedOnAggregateVersion { get; }

        public PlayroomDeletionFailed(Guid playroomId, string reasonCode, int? appliedOnAggregateVersion)
        {
            this.PlayroomId = playroomId;
            this.ReasonCode = reasonCode;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
        }
    }
}