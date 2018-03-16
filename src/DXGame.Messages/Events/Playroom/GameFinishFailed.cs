using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class GameFinishFailed : IEvent
    {
        public GameFinishFailed(Guid playroom, Guid game, string reasonCode)
        {
            this.Playroom = playroom;
            this.Game = game;
            this.ReasonCode = reasonCode;

        }
        public Guid Playroom { get; protected set; }
        public Guid Game { get; protected set; }
        public string ReasonCode { get; protected set; }
    }
}