using System;

namespace DXGame.Messages.Events.Playroom
{
    public class PrivacyChangeFailed : IEvent
    {
        public Guid Playroom { get; }
        public string ReasonCode { get; }

        public PrivacyChangeFailed(Guid playroom, string reasonCode)
        {
            this.Playroom = playroom;
            this.ReasonCode = reasonCode;
        }
    }
}