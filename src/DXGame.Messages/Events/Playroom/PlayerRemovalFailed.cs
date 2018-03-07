using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayerRemovalFailed : ICommandRelatedEvent
    {
        public PlayerRemovalFailed(Guid playroom, Guid player, string reasonCode, Guid relatedCommand)
        {
            this.Playroom = playroom;
            this.Player = player;
            this.ReasonCode = reasonCode;
            this.RelatedCommand = relatedCommand;

        }
        public Guid Playroom { get; }
        public Guid Player { get; }
        public string ReasonCode { get; }
        public Guid RelatedCommand { get; }
    }
}