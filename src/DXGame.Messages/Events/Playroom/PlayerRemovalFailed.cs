using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayerRemovalFailed : IEvent
    {
        public Guid Playroom { get; }
        public Guid Player { get; }
        public string ReasonCode { get; }

        public PlayerRemovalFailed(Guid playroom, Guid player, string reasonCode)
        {
            this.Playroom = playroom;
            this.Player = player;
            this.ReasonCode = reasonCode;
        }
    }
}