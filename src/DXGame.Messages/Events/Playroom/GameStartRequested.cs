using System;
using System.Collections.Generic;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class GameStartRequested : ICommandRelatedEvent
    {
        public GameStartRequested(Guid playroom, Guid game, Guid relatedCommand)
        {
            this.Playroom = playroom;
            this.Game = game;
            this.RelatedCommand = relatedCommand;

        }
        public Guid Playroom { get; protected set; }
        public Guid Game { get; protected set; }
        public IEnumerable<Guid> Players { get; protected set; }
        public Guid RelatedCommand { get; protected set; }
    }
}