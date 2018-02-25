using System;

namespace DXGame.Common.Messages.Events.Playroom
{
    public class PlayroomCreated : IEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public bool IsPrivate { get; }
        public Guid OwnerPlayerId { get; }
    }
}