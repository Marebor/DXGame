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
        public Guid Playroom { get; }
        public Guid Game { get; }
        public string ReasonCode { get; }
        public Guid RelatedCommand { get; }
    }
}