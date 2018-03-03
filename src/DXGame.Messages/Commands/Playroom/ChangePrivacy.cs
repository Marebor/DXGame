using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Commands.Playroom
{
    public class ChangePrivacy : IAuthenticatedCommand
    {
        public Guid Playroom { get; }
        public string Password { get; }
        public Guid Requester { get; }
        public bool Private { get; }

        public ChangePrivacy(Guid playroom, string password, Guid requester, bool @private)
        {
            this.Playroom = playroom;
            this.Password = password;
            this.Requester = requester;
            this.Private = @private;
        }
    }
}