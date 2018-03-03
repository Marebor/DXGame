using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Common.Services;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DXGame.Common.Persistence.MongoDB
{
    public class MongoDBEventStore : IEventStore
    {
        IMongoDatabase _database;
        IMongoCollection<EventEntity> Events
            => _database.GetCollection<EventEntity>("Events");
            
        public MongoDBEventStore(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<IEnumerable<IEvent>> GetAggregateEventsAsync(Guid aggregateId)
            => await Events
                .AsQueryable()
                .Where(e => e.AggregateId == aggregateId)
                .OrderBy(e => e.ExecutionTime)
                .Select(e => e.Content)
                .ToListAsync();

        public async Task<IEnumerable<IEvent>> GetAggregateEventsRangeAsync(Guid aggregateId, DateTime since, DateTime to)
            => await Events
                .AsQueryable()
                .Where(e => e.AggregateId == aggregateId)
                .Where(e => e.ExecutionTime >= since)
                .Where(e => e.ExecutionTime <= to)
                .OrderBy(e => e.ExecutionTime)
                .Select(e => e.Content)
                .ToListAsync();

        public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<IEvent> events)
            => await Events
                .InsertManyAsync(
                    events.Select(e => 
                        new EventEntity(aggregateId, e.GetType().ToString(), DateTime.UtcNow, e)
                    )
                );
    }
}