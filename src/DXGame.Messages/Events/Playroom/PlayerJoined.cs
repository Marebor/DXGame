using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class PlayerJoined : IEvent
    {
        public Guid Playroom { get; }
        public Guid Player { get; }
        public int? AppliedOnAggregateVersion { get; }

        public PlayerJoined(Guid playroom, Guid player, int? appliedOnAggregateVersion)
        {
            this.Playroom = playroom;
            this.Player = player;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
        }
    }
}