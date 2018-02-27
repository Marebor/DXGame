using System;

namespace DXGame.Common.Messages.Events.Playroom
{
    public class PasswordChangeFailed : IEvent
    {
        public Guid Playroom { get; }
        public string ReasonCode { get; }

        public PasswordChangeFailed(Guid playroom, string reasonCode)
        {
            this.Playroom = playroom;
            this.ReasonCode = reasonCode;
        }
    }
}