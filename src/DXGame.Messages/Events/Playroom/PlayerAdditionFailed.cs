using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayerAdditionFailed : ICommandRelatedEvent
    {
        public PlayerAdditionFailed(Guid playroom, Guid player, string reasonCode, Guid relatedCommand)
        {
            this.Playroom = playroom;
            this.Player = player;
            this.ReasonCode = reasonCode;
            this.RelatedCommand = relatedCommand;

        }
        public Guid Playroom { get; protected set; }
        public Guid Player { get; protected set; }
        public string ReasonCode { get; protected set; }
        public Guid RelatedCommand { get; protected set; }
    }
}