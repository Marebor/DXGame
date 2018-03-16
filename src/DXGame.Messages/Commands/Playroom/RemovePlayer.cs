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
        public Guid CommandId { get; protected set; }
        public Guid Playroom { get; protected set; }
        public Guid Player { get; protected set; }
        public Guid Requester { get; protected set; }
    }
}