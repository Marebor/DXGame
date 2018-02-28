using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DXGame.Common.Helpers;
using DXGame.Common.Services;
using DXGame.Messages.Events;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DXGame.Common.Persistence
{
    public class MongoDBEventStore : IEventStore
    {
        IMongoDatabase _database;
        ISerializer _serializer;

        IMongoCollection<EventEntity> Events
            => _database.GetCollection<EventEntity>("Events");
            
        public MongoDBEventStore(IMongoDatabase database, ISerializer serializer)
        {
            _database = database;
            _serializer = serializer;
        }

        public async Task<IEnumerable<IEvent>> GetAggregateEventsAsync(Guid aggregateId)
            => await Events
                .AsQueryable()
                .Where(e => e.AggregateId == aggregateId)
                .OrderBy(e => e.ExecutionTime)
                .Select(e => _serializer.Deserialize<IEvent>(e.Content))
                .ToListAsync();

        public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<IEvent> events)
            => await Events
                .InsertManyAsync(events
                    .Select(e => 
                        new EventEntity(aggregateId, e.GetType().ToString(), DateTime.UtcNow, _serializer.Serialize(e))
                    )
                );
    }
}