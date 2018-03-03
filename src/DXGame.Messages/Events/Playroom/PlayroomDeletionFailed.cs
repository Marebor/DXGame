using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayroomDeletionFailed : IEvent
    {
        public Guid PlayroomId { get; }
        public string ReasonCode { get; }

        public PlayroomDeletionFailed(Guid playroomId, string reasonCode)
        {
            this.PlayroomId = playroomId;
            this.ReasonCode = reasonCode;
        }
    }
}