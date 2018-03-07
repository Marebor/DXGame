using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayerJoined : ICommandRelatedEvent, IAggregateAppliedEvent
    {
        public PlayerJoined(Guid playroom, Guid player, int appliedOnAggregateVersion, Guid relatedCommand)
        {
            this.Playroom = playroom;
            this.Player = player;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
            this.RelatedCommand = relatedCommand;

        }
        public Guid Playroom { get; }
        public Guid Player { get; }
        public int AppliedOnAggregateVersion { get; }
        public Guid RelatedCommand { get; }
    }
}