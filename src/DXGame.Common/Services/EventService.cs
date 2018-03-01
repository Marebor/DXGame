using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DXGame.Common.Persistence;
using DXGame.Messages.Events;
using RawRabbit;

namespace DXGame.Common.Services
{
    public class EventService : IEventService
    {
        IEventStore _eventStore;
        IBusClient _busClient;

        public EventService(IEventStore eventStore, IBusClient busClient)
        {
            _eventStore = eventStore;
            _busClient = busClient;
        }

        public async Task<IEnumerable<IEvent>> GetAggregateEventsAsync(Guid aggregateId)
            => await _eventStore.GetAggregateEventsAsync(aggregateId);

        public async Task PublishEventsAsync(params IEvent[] events)
            => await Task.WhenAll(events.Select(async e => await _busClient.PublishAsync(e)));

        public async Task StoreEventsAsync(Guid aggregateId, params IEvent[] events)
            => await _eventStore.SaveEventsAsync(aggregateId, events);
    }
}