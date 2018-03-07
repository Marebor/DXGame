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
        public Guid Playroom { get; }
        public Guid Game { get; }
        public Guid RelatedCommand { get; }
    }
}