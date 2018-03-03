using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DXGame.Common.Communication;

namespace DXGame.Common.Services
{
    public interface IEventService
    {
        Task StoreEventsAsync(Guid aggregateId, params IEvent[] events);
        Task PublishEventsAsync(params IEvent[] events);
        Task<IEnumerable<IEvent>> GetAggregateEventsAsync(Guid aggregateId);
    }
}