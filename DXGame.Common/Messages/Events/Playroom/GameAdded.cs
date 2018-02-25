using System;

namespace DXGame.Common.Messages.Events.Playroom
{
    public class GameAdded : IEvent
    {
        public Guid Playroom { get; }
        public Guid Game { get; }

        public GameAdded(Guid playroom, Guid game) 
        {
            Playroom = playroom;
            Game = game;
        }
    }
}