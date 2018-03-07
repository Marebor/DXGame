using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayroomCreationFailed : IEvent
    {
        public Guid PlayroomId { get; }
        public string ReasonCode { get; }
        public int? AppliedOnAggregateVersion { get; }

        public PlayroomCreationFailed(Guid playroomId, string reasonCode, int? appliedOnAggregateVersion)
        {
            this.PlayroomId = playroomId;
            this.ReasonCode = reasonCode;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
        }
    }
}