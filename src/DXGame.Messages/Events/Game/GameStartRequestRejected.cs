using System;

namespace DXGame.Messages.Events.Game
{
    public class GameStartRequestRejected : IEvent
    {
        public Guid Playroom { get; }
        public Guid Game { get; }

        public GameStartRequestRejected(Guid playroom, Guid game)
        {
            this.Playroom = playroom;
            this.Game = game;
        }
    }
}