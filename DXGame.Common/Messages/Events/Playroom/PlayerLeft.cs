using System;

namespace DXGame.Common.Messages.Events.Playroom
{
    public class PlayerLeft : IEvent
    {
        public Guid Playroom { get; }
        public Guid Player { get; }

        public PlayerLeft(Guid playroom, Guid player) 
        {
            Playroom = playroom;
            Player = player;
        }
    }
}