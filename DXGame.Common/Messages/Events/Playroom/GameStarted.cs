using System;

namespace DXGame.Common.Messages.Events.Playroom
{
    public class GameStarted : IEvent
    {
        public Guid Playroom { get; }
        public Guid Game { get; }

        public GameStarted(Guid playroom, Guid game)
        {
            this.Playroom = playroom;
            this.Game = game;
        }
    }
}