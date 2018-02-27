using System;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayerJoined : IEvent
    {
        public Guid Playroom { get; }
        public Guid Player { get; }

        public PlayerJoined(Guid playroom, Guid player) 
        {
            this.Playroom = playroom;
            this.Player = player;
        }
    }
}