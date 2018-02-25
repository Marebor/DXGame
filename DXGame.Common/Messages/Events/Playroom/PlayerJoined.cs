using System;

namespace DXGame.Common.Messages.Events.Playroom
{
    public class PlayerJoined : IEvent
    {
        public Guid Playroom { get; }
        public Guid Player { get; }

        public PlayerJoined(Guid playroom, Guid player) 
        {
            Playroom = playroom;
            Player = player;
        }
    }
}