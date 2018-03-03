using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Commands.Playroom
{
    public class DeletePlayroom : IAuthenticatedCommand
    {
        public Guid Playroom { get; }
        public string Password { get; }
        public Guid Requester { get; }

        public DeletePlayroom(Guid playroom, string password, Guid requester)
        {
            this.Playroom = playroom;
            this.Password = password;
            this.Requester = requester;
        }
    }
}