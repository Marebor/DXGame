using System;

namespace DXGame.Common.Messages.Events.Playroom
{
    public class PlayroomCreated : IEvent
    {
        public Guid PlayroomId { get; }

        public PlayroomCreated(Guid playroomId)
        {
            this.PlayroomId = playroomId;
        }
    }
}