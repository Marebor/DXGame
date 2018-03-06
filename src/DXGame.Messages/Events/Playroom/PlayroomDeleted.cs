using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayroomDeleted : IEvent
    {
        public Guid Id { get; }
        public int? AppliedOnAggregateVersion { get; }

        public PlayroomDeleted(Guid id, int? appliedOnAggregateVersion)
        {
            this.Id = id;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
        }
    }
}