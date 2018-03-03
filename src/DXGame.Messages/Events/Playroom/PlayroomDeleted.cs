using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayroomDeleted : IEvent
    {
        public Guid Id { get; }

        public PlayroomDeleted(Guid id)
        {
            this.Id = id;
        }
    }
}