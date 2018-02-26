using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DXGame.Common.Messages.Events;

namespace DXGame.Common.Persistence
{
    public interface IEventStore
    {
         Task<IEnumerable<IEvent>> GetAggregateEventsAsync(Guid aggregateId);
         Task SaveEventsAsync(IEnumerable<IEvent> events);
    }
}