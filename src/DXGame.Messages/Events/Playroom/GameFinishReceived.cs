using System;

namespace DXGame.Messages.Events.Playroom
{
    public class GameFinishReceived : IEvent
    {
        public Guid Playroom { get; }
        public Guid Game { get; }

        public GameFinishReceived(Guid playroom, Guid game)
        {
            this.Playroom = playroom;
            this.Game = game;
        }
    }
}