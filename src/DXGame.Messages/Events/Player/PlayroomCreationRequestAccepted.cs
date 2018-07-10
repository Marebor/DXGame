using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Player
{
    public class PlayroomCreationRequestAccepted : ICommandRelatedEvent
    {
        public PlayroomCreationRequestAccepted(Guid playroomId, Guid relatedCommand)
        {
            this.PlayroomId = playroomId;
            this.RelatedCommand = relatedCommand;
        }

        public Guid PlayroomId { get; protected set; }
        public Guid RelatedCommand { get; protected set; }
    }
}