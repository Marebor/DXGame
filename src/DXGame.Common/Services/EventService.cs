using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DXGame.Common.Persistence;
using DXGame.Common.Communication;
using RawRabbit;

namespace DXGame.Common.Services
{
    public class EventService : IEventService
    {
        IEventStore _eventStore;
        IMessageBus _bus;

        public EventService(IEventStore eventStore, IMessageBus bus)
        {
            _eventStore = eventStore;
            _bus = bus;
        }

        public async Task<IEnumerable<IEvent>> GetAggregateEventsAsync(Guid aggregateId)
            => await _eventStore.GetAggregateEventsAsync(aggregateId);

        public async Task PublishEventsAsync(params IEvent[] events)
            => await Task.WhenAll(events.Select(async e => await _bus.PublishAsync(e)));

        public async Task StoreEventsAsync(Guid aggregateId, params IEvent[] events)
            => await _eventStore.SaveEventsAsync(aggregateId, events);
    }
}