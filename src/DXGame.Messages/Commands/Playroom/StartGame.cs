using System;

namespace DXGame.Messages.Commands.Playroom
{
    public class StartGame : IAuthenticatedCommand
    {
        public Guid Playroom { get; }
        public Guid Game { get; }
        public Guid Requester { get; }

        public StartGame(Guid playroom, Guid game, Guid requester)
        {
            this.Playroom = playroom;
            this.Game = game;
            this.Requester = requester;
        }
    }
}