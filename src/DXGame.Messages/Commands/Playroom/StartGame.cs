using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Commands.Playroom
{
    public class StartGame : IAuthenticatedCommand
    {
        public StartGame(Guid commandId, Guid playroom, Guid game, Guid requester)
        {
            this.CommandId = commandId;
            this.Playroom = playroom;
            this.Game = game;
            this.Requester = requester;

        }
        public Guid CommandId { get; }
        public Guid Playroom { get; }
        public Guid Game { get; }
        public Guid Requester { get; }
    }
}