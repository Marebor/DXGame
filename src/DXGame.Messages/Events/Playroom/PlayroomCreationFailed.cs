using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayroomCreationFailed : ICommandRelatedEvent
    {
        public PlayroomCreationFailed(Guid playroomId, string reasonCode, Guid relatedCommand)
        {
            this.PlayroomId = playroomId;
            this.ReasonCode = reasonCode;
            this.RelatedCommand = relatedCommand;

        }
        public Guid PlayroomId { get; protected set; }
        public string ReasonCode { get; protected set; }
        public int? AppliedOnAggregateVersion { get; protected set; }
        public Guid RelatedCommand { get; protected set; }
    }
}