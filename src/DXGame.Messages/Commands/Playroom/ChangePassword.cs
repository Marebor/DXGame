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
        public Guid CommandId { get; }
        public Guid Playroom { get; }
        public string OldPassword { get; }
        public string NewPassword { get; }
        public Guid Requester { get; }
    }
}