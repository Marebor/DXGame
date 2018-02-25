using System;

namespace DXGame.Common.Messages.Events.Playroom
{
    public class PasswordChanged : IEvent
    {
        public Guid Playroom { get; }

        public PasswordChanged(Guid playroom)
        {
            this.Playroom = playroom;
        }
    }
}