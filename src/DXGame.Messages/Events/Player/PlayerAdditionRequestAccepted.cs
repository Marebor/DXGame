using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Player
{
    public class PlayerAdditionRequestAccepted : ICommandRelatedEvent
    {
        public PlayerAdditionRequestAccepted(Guid playroomId, Guid playerId, Guid relatedCommand)
        {
            this.PlayroomId = playroomId;
            this.PlayerId = playerId;
            this.RelatedCommand = relatedCommand;
        }

        public Guid PlayroomId { get; protected set; }
        public Guid PlayerId { get; protected set; }
        public Guid RelatedCommand { get; protected set; }
    }
}