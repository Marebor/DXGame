using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class OwnerChanged : ICommandRelatedEvent, IAggregateAppliedEvent
    {
        public OwnerChanged(Guid playroom, Guid owner, int appliedOnAggregateVersion, Guid relatedCommand)
        {
            this.Playroom = playroom;
            this.Owner = owner;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
            this.RelatedCommand = relatedCommand;

        }
        public Guid Playroom { get; protected set; }
        public Guid Owner { get; protected set; }
        public int AppliedOnAggregateVersion { get; protected set; }
        public Guid RelatedCommand { get; protected set; }
    }
}