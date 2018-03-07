using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class GameStarted : ICommandRelatedEvent, IAggregateAppliedEvent
    {
        public GameStarted(Guid playroom, Guid game, int appliedOnAggregateVersion, Guid relatedCommand)
        {
            this.Playroom = playroom;
            this.Game = game;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
            this.RelatedCommand = relatedCommand;

        }
        public Guid Playroom { get; }
        public Guid Game { get; }
        public int AppliedOnAggregateVersion { get; }
        public Guid RelatedCommand { get; }
    }
}