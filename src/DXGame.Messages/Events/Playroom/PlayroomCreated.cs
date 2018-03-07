using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayroomCreated : IEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public bool IsPrivate { get; }
        public Guid Owner { get; }
        public string Password { get; }
        public int? AppliedOnAggregateVersion { get; }

        public PlayroomCreated(Guid id, string name, bool isPrivate, Guid owner, string password, int? appliedOnAggregateVersion)
        {
            this.Id = id;
            this.Name = name;
            this.IsPrivate = isPrivate;
            this.Owner = owner;
            this.Password = password;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
        }
    }
}