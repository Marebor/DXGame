using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Player
{
    public class PlayerCreated : ICommandRelatedEvent, IAggregateAppliedEvent
    {
        public PlayerCreated(Guid id, string name, string password, int appliedOnAggregateVersion, Guid relatedCommand)
        {
            this.Id = id;
            this.Name = name;
            this.Password = password;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
            this.RelatedCommand = relatedCommand;
        }
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Password { get; protected set; }
        public int AppliedOnAggregateVersion { get; protected set; }
        public Guid RelatedCommand { get; protected set; }

        public void HidePassword() => Password = new string('*', Password.Length);
    }
}