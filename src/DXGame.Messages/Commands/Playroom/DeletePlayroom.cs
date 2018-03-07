using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Commands.Playroom
{
    public class DeletePlayroom : IAuthenticatedCommand
    {
        public DeletePlayroom(Guid commandId, Guid playroom, string password, Guid requester)
        {
            this.CommandId = commandId;
            this.Playroom = playroom;
            this.Password = password;
            this.Requester = requester;

        }
        public Guid CommandId { get; }
        public Guid Playroom { get; }
        public string Password { get; }
        public Guid Requester { get; }
    }
}