using System;

namespace DXGame.Common.Messages.Commands.Playroom
{
    public class ChangePrivacy : ICommand
    {
        public Guid Playroom { get; }
        public string Password { get; }

        public ChangePrivacy(Guid playroom, string password)
        {
            this.Playroom = playroom;
            this.Password = password;
        }
    }
}