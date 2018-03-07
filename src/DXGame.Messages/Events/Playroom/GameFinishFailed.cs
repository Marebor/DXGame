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
        public Guid Playroom { get; }
        public Guid Game { get; }
        public string ReasonCode { get; }
    }
}