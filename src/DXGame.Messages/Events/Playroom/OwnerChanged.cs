using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Events.Playroom
{
    public class OwnerChanged : IEvent
    {
        public Guid Playroom { get; }
        public Guid Owner { get; }

        public OwnerChanged(Guid playroom, Guid owner) 
        {
            this.Playroom = playroom;
            this.Owner = owner;
        }
    }
}