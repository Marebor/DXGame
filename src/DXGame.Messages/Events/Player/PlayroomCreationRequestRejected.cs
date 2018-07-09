using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Player
{
    public class PlayroomCreationRequestRejected : ICommandRelatedEvent
    {
        public PlayroomCreationRequestRejected(Guid playroomId, string reasonCode, Guid relatedCommand)
        {
            this.PlayroomId = playroomId;
            this.ReasonCode = reasonCode;
            this.RelatedCommand = relatedCommand;
        }

        public Guid PlayroomId { get; protected set; }
        public string ReasonCode { get; protected set; }
        public Guid RelatedCommand { get; protected set; }
    }
}