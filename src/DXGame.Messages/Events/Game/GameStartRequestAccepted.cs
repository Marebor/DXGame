using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Game
{
    public class GameStartRequestAccepted : ICommandRelatedEvent
    {
        public GameStartRequestAccepted(Guid playroom, Guid game, Guid relatedCommand)
        {
            this.Playroom = playroom;
            this.Game = game;
            this.RelatedCommand = relatedCommand;

        }
        public Guid Playroom { get; protected set; }
        public Guid Game { get; protected set; }
        public Guid RelatedCommand { get; protected set; }
    }
}