using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayerAdditionRequested : ICommandRelatedEvent
    {
        public Guid PlayroomId { get; protected set; }
        public Guid PlayerId { get; protected set; }
        public Guid RelatedCommand { get; protected set; }

        public PlayerAdditionRequested(Guid playroomId, Guid playerId, Guid relatedCommand)
        {
            this.PlayroomId = playroomId;
            this.PlayerId = playerId;
            this.RelatedCommand = relatedCommand;
        }
    }
}