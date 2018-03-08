using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayroomDeleted : ICommandRelatedEvent, IAggregateAppliedEvent
    {
        public PlayroomDeleted(Guid playroom, int appliedOnAggregateVersion, Guid relatedCommand)
        {
            this.Playroom = playroom;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
            this.RelatedCommand = relatedCommand;

        }
        public Guid Playroom { get; }
        public int AppliedOnAggregateVersion { get; }
        public Guid RelatedCommand { get; }
    }
}