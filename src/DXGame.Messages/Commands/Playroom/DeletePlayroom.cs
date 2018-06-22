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
        public Guid CommandId { get; protected set; }
        public Guid Playroom { get; protected set; }
        public string Password { get; protected set; }
        public Guid Requester { get; protected set; }
    }
}