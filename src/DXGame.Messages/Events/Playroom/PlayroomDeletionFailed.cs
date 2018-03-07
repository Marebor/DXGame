using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayroomDeletionFailed : ICommandRelatedEvent
    {
        public PlayroomDeletionFailed(Guid playroomId, string reasonCode, Guid relatedCommand)
        {
            this.PlayroomId = playroomId;
            this.ReasonCode = reasonCode;
            this.RelatedCommand = relatedCommand;

        }
        public Guid PlayroomId { get; }
        public string ReasonCode { get; }
        public Guid RelatedCommand { get; }
    }
}