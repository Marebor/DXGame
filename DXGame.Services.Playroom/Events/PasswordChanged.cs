using System;
using DXGame.Common.Messages.Events;

namespace DXGame.Services.Playroom.Events
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