using System;

namespace DXGame.Common.Messages.Commands.Playroom
{
    public class StartGame : ICommand
    {
        public Guid Playroom { get; }
        public Guid Game { get; }

        public StartGame(Guid playroom, Guid game)
        {
            this.Playroom = playroom;
            this.Game = game;
        }
    }
}