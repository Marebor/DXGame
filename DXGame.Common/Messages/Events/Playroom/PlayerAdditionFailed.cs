using System;

namespace DXGame.Common.Messages.Events.Playroom
{
    public class PlayerAdditionFailed : IEvent
    {
        public Guid Playroom { get; }
        public Guid Player { get; }
        public string ReasonCode { get; }

        public PlayerAdditionFailed(Guid playroom, Guid player, string reasonCode)
        {
            this.Playroom = playroom;
            this.Player = player;
            this.ReasonCode = reasonCode;
        }
    }
}