using System;

namespace DXGame.Common.Messages.Events.Playroom
{
    public class PlayroomCreated : IEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public bool IsPrivate { get; }
        public Guid Owner { get; }
        public string Password { get; }

        public PlayroomCreated(Guid id, string name, bool isPrivate, Guid owner, string password)
        {
            this.Id = id;
            this.Name = name;
            this.IsPrivate = isPrivate;
            this.Owner = owner;
            this.Password = password;
        }
    }
}