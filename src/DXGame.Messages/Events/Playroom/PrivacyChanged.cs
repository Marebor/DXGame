using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PrivacyChanged : ICommandRelatedEvent, IAggregateAppliedEvent
    {
        public PrivacyChanged(Guid playroom, bool isPrivate, int appliedOnAggregateVersion, Guid relatedCommand)
        {
            this.Playroom = playroom;
            this.IsPrivate = isPrivate;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
            this.RelatedCommand = relatedCommand;

        }
        public Guid Playroom { get; }
        public bool IsPrivate { get; }
        public int AppliedOnAggregateVersion { get; }
        public Guid RelatedCommand { get; }
    }
}