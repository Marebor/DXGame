using System;

namespace DXGame.Messages.Abstract
{
    public interface ICommandRelatedEvent : IEvent
    {
        Guid RelatedCommand { get; }
    }
}