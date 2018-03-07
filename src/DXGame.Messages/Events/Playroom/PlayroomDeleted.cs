using System;
using DXGame.Messages.Abstract;

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