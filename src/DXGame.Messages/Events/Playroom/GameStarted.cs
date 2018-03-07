using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class GameStarted : IEvent
    {
        public Guid Playroom { get; }
        public Guid Game { get; }
        public int? AppliedOnAggregateVersion { get; }

        public GameStarted(Guid playroom, Guid game, int? appliedOnAggregateVersion)
        {
            this.Playroom = playroom;
            this.Game = game;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
        }
    }
}