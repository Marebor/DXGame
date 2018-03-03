using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Events.Playroom
{
    public class PrivacyChanged : IEvent
    {
        public Guid Playroom { get; }

        public bool IsPrivate { get; }

        public PrivacyChanged(Guid playroom, bool isPrivate)
        {
            this.Playroom = playroom;
            this.IsPrivate = isPrivate;
        }
    }
}