using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Commands.Playroom
{
    public class ChangePassword : IAuthenticatedCommand
    {
        public ChangePassword(Guid commandId, Guid playroom, string oldPassword, string newPassword, Guid requester)
        {
            this.CommandId = commandId;
            this.Playroom = playroom;
            this.OldPassword = oldPassword;
            this.NewPassword = newPassword;
            this.Requester = requester;

        }
        public Guid CommandId { get; protected set; }
        public Guid Playroom { get; protected set; }
        public string OldPassword { get; protected set; }
        public string NewPassword { get; protected set; }
        public Guid Requester { get; protected set; }
    }
}