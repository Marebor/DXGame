using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Player
{
    public class PlayerAdditionRequestRejected : ICommandRelatedEvent
    {
        public PlayerAdditionRequestRejected(Guid playroomId, Guid playerId, string reasonCode, Guid relatedCommand)
        {
            this.PlayroomId = playroomId;
            this.PlayerId = playerId;
            this.ReasonCode = reasonCode;
            this.RelatedCommand = relatedCommand;
        }

        public Guid PlayroomId { get; protected set; }
        public Guid PlayerId { get; protected set; }
        public string ReasonCode { get; protected set; }
        public Guid RelatedCommand { get; protected set; }
    }
}