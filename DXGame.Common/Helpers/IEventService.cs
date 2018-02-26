using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DXGame.Common.Messages.Events;

namespace DXGame.Common.Helpers
{
    public interface IEventService
    {
        Task SaveEvents(params IEvent[] events);
        Task PublishEvents(params IEvent[] events);
        Task<IEnumerable<IEvent>> GetAggregateEventsAsync(Guid aggregateId);
    }
}