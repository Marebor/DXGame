using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Commands.Playroom
{
    public class ChangeOwner : IAuthenticatedCommand
    {
        public ChangeOwner(Guid commandId, Guid playroom, Guid requester, string password, Guid newOwner)
        {
            this.CommandId = commandId;
            this.Playroom = playroom;
            this.Requester = requester;
            this.Password = password;
            this.NewOwner = newOwner;

        }
        public Guid CommandId { get; }
        public Guid Playroom { get; }
        public Guid Requester { get; }
        public string Password { get; }
        public Guid NewOwner { get; }
    }
}