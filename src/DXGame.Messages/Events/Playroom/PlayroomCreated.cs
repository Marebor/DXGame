using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayroomCreated : ICommandRelatedEvent, IAggregateAppliedEvent
    {
        public PlayroomCreated(Guid id, string name, bool isPrivate, Guid owner, int appliedOnAggregateVersion, Guid relatedCommand)
        {
            this.Id = id;
            this.Name = name;
            this.IsPrivate = isPrivate;
            this.Owner = owner;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
            this.RelatedCommand = relatedCommand;
        }
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public bool IsPrivate { get; protected set; }
        public Guid Owner { get; protected set; }
        public int AppliedOnAggregateVersion { get; protected set; }
        public Guid RelatedCommand { get; protected set; }
    }
}