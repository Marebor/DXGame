using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Events.Playroom
{
    public class PasswordChanged : IEvent
    {
        public Guid Playroom { get; }
        public string Password { get; }
        public int? AppliedOnAggregateVersion { get; }

        public PasswordChanged(Guid playroom, string password, int? appliedOnAggregateVersion)
        {
            this.Playroom = playroom;
            this.Password = password;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
        }
    }
}