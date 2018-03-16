using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class GameStartFailed : ICommandRelatedEvent
    {
        public GameStartFailed(Guid playroom, Guid game, string reasonCode, Guid relatedCommand)
        {
            this.Playroom = playroom;
            this.Game = game;
            this.ReasonCode = reasonCode;
            this.RelatedCommand = relatedCommand;

        }
        public Guid Playroom { get; protected set; }
        public Guid Game { get; protected set; }
        public string ReasonCode { get; protected set; }
        public Guid RelatedCommand { get; protected set; }
    }
}