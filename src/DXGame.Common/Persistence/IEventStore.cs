using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DXGame.Messages.Abstract;

namespace DXGame.Common.Persistence
{
    public interface IEventStore
    {
         Task<IEnumerable<IEvent>> GetAllEventsAsync();
         Task<IEnumerable<IEvent>> GetAllEventsTimeRangeAsync(DateTime since, DateTime to);
         Task<IEnumerable<IEvent>> GetAggregateEventsAsync(Guid aggregateId);
         Task<IEnumerable<IEvent>> GetAggregateEventsTimeRangeAsync(Guid aggregateId, DateTime since, DateTime to);
         Task<IEnumerable<IEvent>> GetAggregateEventsVersionRangeAsync(Guid aggregateId, int since, int to);
         Task SaveEventsAsync(Guid aggregateId, IEnumerable<IEvent> events);
         Task VerifyAggregateVersionAsync(Guid aggregateId, int version);
    }
}