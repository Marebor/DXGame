using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Player
{
    public class PlayerCreationFailed : ICommandRelatedEvent
    {
        public PlayerCreationFailed(Guid player, string reasonCode, Guid relatedCommand)
        {
            this.Player = player;
            this.ReasonCode = reasonCode;
            this.RelatedCommand = relatedCommand;

        }
        public Guid Player { get; protected set; }
        public string ReasonCode { get; protected set; }
        public int? AppliedOnAggregateVersion { get; protected set; }
        public Guid RelatedCommand { get; protected set; }
    }
}