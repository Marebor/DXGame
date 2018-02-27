using System;
using System.Collections.Generic;

namespace DXGame.Common.Messages.Events.Playroom
{
    public class GameStartRequested : IEvent
    {
        public Guid Playroom { get; }
        public Guid Game { get; }
        public IEnumerable<Guid> Players { get; }

        public GameStartRequested(Guid playroom, Guid game, IEnumerable<Guid> players) 
        {
            this.Playroom = playroom;
            this.Game = game;
            this.Players = players;
        }
    }
}