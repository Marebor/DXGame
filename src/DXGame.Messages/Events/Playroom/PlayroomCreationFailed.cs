using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayroomCreationFailed : IEvent
    {
        public Guid PlayroomId { get; }
        public string ReasonCode { get; }

        public PlayroomCreationFailed(Guid playroomId, string reasonCode)
        {
            this.PlayroomId = playroomId;
            this.ReasonCode = reasonCode;
        }
    }
}