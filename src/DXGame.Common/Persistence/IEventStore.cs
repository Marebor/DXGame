using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DXGame.Messages.Events;

namespace DXGame.Common.Persistence
{
    public interface IEventStore
    {
         Task<IEnumerable<IEvent>> GetAggregateEventsAsync(Guid aggregateId);
         Task<IEnumerable<IEvent>> GetAggregateEventsRangeAsync(Guid aggregateId, DateTime since, DateTime to);
         Task SaveEventsAsync(Guid aggregateId, IEnumerable<IEvent> events);
    }
}