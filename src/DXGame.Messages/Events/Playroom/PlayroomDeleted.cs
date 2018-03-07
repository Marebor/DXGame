using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayroomDeleted : ICommandRelatedEvent, IAggregateAppliedEvent
    {
        public PlayroomDeleted(Guid id, int appliedOnAggregateVersion, Guid relatedCommand)
        {
            this.Id = id;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
            this.RelatedCommand = relatedCommand;

        }
        public Guid Id { get; }
        public int AppliedOnAggregateVersion { get; }
        public Guid RelatedCommand { get; }
    }
}