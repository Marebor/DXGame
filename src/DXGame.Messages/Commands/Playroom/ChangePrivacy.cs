using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Commands.Playroom
{
    public class ChangePrivacy : IAuthenticatedCommand
    {
        public ChangePrivacy(Guid commandId, Guid playroom, string password, Guid requester, bool @private) 
        {
            this.CommandId = commandId;
            this.Playroom = playroom;
            this.Password = password;
            this.Requester = requester;
            this.Private = @private;
               
        }
        public Guid CommandId { get; protected set; }
        public Guid Playroom { get; protected set; }
        public string Password { get; protected set; }
        public Guid Requester { get; protected set; }
        public bool Private { get; protected set; }
    }
}