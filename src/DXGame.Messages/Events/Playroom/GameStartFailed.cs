using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Events.Playroom
{
    public class GameStartFailed : IEvent
    {
        public Guid Playroom { get; }
        public Guid Game { get; }
        public string ReasonCode { get; }

        public GameStartFailed(Guid playroom, Guid game, string reasonCode) 
        {
            this.Playroom = playroom;
            this.Game = game;
            this.ReasonCode = reasonCode;
        }
    }
}