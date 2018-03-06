using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Events.Playroom
{
    public class PrivacyChanged : IEvent
    {
        public Guid Playroom { get; }

        public bool IsPrivate { get; }
        public int? AppliedOnAggregateVersion { get; }

        public PrivacyChanged(Guid playroom, bool isPrivate, int? appliedOnAggregateVersion)
        {
            this.Playroom = playroom;
            this.IsPrivate = isPrivate;
            this.AppliedOnAggregateVersion = appliedOnAggregateVersion;
        }
    }
}