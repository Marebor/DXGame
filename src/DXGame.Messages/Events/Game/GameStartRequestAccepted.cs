using System;

namespace DXGame.Messages.Events.Game
{
    public class GameStartRequestAccepted : IEvent
    {
        public Guid Playroom { get; }
        public Guid Game { get; }

        public GameStartRequestAccepted(Guid playroom, Guid game)
        {
            this.Playroom = playroom;
            this.Game = game;
        }
    }
}