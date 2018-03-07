using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Events.Playroom
{
    public class OwnerChanged : IEvent
    {
        public Guid Playroom { get; }
        public Guid Owner { get; }
        public int? AppliedOnAggregateVersion { get; }

        public OwnerChanged(Guid playroom, Guid owner, int? appliedOnAggregateVersion)
        {
            this.Playroom = playroom;
            this.Owner = owner;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
        }
    }
}