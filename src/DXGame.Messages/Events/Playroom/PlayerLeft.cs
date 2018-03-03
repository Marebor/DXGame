using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayerLeft : IEvent
    {
        public Guid Playroom { get; }
        public Guid Player { get; }

        public PlayerLeft(Guid playroom, Guid player) 
        {
            this.Playroom = playroom;
            this.Player = player;
        }
    }
}