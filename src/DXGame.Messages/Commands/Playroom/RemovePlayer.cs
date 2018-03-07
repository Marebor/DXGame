using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Commands.Playroom
{
    public class RemovePlayer : IAuthenticatedCommand
    {
        public RemovePlayer(Guid commandId, Guid playroom, Guid player, Guid requester)
        {
            this.CommandId = commandId;
            this.Playroom = playroom;
            this.Player = player;
            this.Requester = requester;

        }
        public Guid CommandId { get; }
        public Guid Playroom { get; }
        public Guid Player { get; }
        public Guid Requester { get; }
    }
}