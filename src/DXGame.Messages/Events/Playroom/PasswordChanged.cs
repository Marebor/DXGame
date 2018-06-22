using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PasswordChanged : ICommandRelatedEvent, IAggregateAppliedEvent
    {
        public PasswordChanged(Guid playroom, string password, int appliedOnAggregateVersion, Guid relatedCommand)
        {
            this.Playroom = playroom;
            this.Password = password;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
            this.RelatedCommand = relatedCommand;

        }
        public Guid Playroom { get; protected set; }
        public string Password { get; protected set; }
        public int AppliedOnAggregateVersion { get; protected set; }
        public Guid RelatedCommand { get; protected set; }

        public void HidePassword() => Password = new string('*', Password.Length);
    }
}