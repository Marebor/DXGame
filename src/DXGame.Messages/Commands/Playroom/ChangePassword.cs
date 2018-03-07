using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Commands.Playroom
{
    public class ChangePassword : IAuthenticatedCommand
    {
        public Guid Playroom { get; }
        public string OldPassword { get; }
        public string NewPassword { get; }
        public Guid Requester { get; }

        public ChangePassword(Guid playroom, string oldPassword, string newPassword, Guid requester)
        {
            this.Playroom = playroom;
            this.OldPassword = oldPassword;
            this.NewPassword = newPassword;
            this.Requester = requester;
        }
    }
}