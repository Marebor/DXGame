using System;

namespace DXGame.Messages.Events.Playroom
{
    public class PasswordChanged : IEvent
    {
        public Guid Playroom { get; }
        public string Password { get; }

        public PasswordChanged(Guid playroom, string password)
        {
            this.Playroom = playroom;
            this.Password = password;
        }
    }
}