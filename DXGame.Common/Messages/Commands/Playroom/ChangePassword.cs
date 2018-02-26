using System;

namespace DXGame.Common.Messages.Commands.Playroom
{
    public class ChangePassword : ICommand
    {
        public Guid Playroom { get; }
        public string OldPassword { get; }
        public string NewPassword { get; }

        public ChangePassword(Guid playroom, string oldPassword, string newPassword)
        {
            this.Playroom = playroom;
            this.OldPassword = oldPassword;
            this.NewPassword = newPassword;
        }
    }
}