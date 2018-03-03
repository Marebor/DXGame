using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Commands.Playroom
{
    public class ChangeOwner : IAuthenticatedCommand
    {
        public Guid Playroom { get; }
        public Guid Requester { get; }
        public string Password { get; }
        public Guid NewOwner { get; }

        public ChangeOwner(Guid playroom, Guid requester, string password, Guid newOwner)
        {
            this.Playroom = playroom;
            this.Requester = requester;
            this.Password = password;
            this.NewOwner = newOwner;
        }
    }
}