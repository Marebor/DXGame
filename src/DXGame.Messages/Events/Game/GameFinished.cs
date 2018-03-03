using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Events.Game
{
    public class GameFinished : IEvent
    {
        public Guid Playroom { get; }
        public Guid Game { get; }

        public GameFinished(Guid playroom, Guid game)
        {
            this.Playroom = playroom;
            this.Game = game;
        }
    }
}