using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayroomCreated : ICommandRelatedEvent, IAggregateAppliedEvent
    {
        public PlayroomCreated(Guid id, string name, bool isPrivate, Guid owner, string password, int appliedOnAggregateVersion, Guid relatedCommand)
        {
            this.Id = id;
            this.Name = name;
            this.IsPrivate = isPrivate;
            this.Owner = owner;
            this.Password = password;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
            this.RelatedCommand = relatedCommand;

        }
        public Guid Id { get; }
        public string Name { get; }
        public bool IsPrivate { get; }
        public Guid Owner { get; }
        public string Password { get; }
        public int AppliedOnAggregateVersion { get; }
        public Guid RelatedCommand { get; }
    }
}