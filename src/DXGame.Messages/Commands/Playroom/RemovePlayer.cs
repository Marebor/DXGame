using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Commands.Playroom
{
    public class RemovePlayer : IAuthenticatedCommand
    {
        public Guid Playroom { get; }
        public Guid Player { get; }
        public Guid Requester { get; }

        public RemovePlayer(Guid playroom, Guid player, Guid requester)
        {
            this.Playroom = playroom;
            this.Player = player;
            this.Requester = requester;
        }
    }
}