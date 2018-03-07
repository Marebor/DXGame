using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PasswordChangeFailed : ICommandRelatedEvent
    {
        public PasswordChangeFailed(Guid playroom, string reasonCode, Guid relatedCommand)
        {
            this.Playroom = playroom;
            this.ReasonCode = reasonCode;
            this.RelatedCommand = relatedCommand;

        }
        public Guid Playroom { get; }
        public string ReasonCode { get; }
        public Guid RelatedCommand { get; }
    }
}