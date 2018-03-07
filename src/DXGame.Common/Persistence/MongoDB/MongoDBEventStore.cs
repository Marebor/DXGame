using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DXGame.Common.Exceptions;
using DXGame.Common.Helpers;
using DXGame.Common.Services;
using DXGame.Messages.Abstract;
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

        public async Task<IEnumerable<IEvent>> GetAggregateEventsTimeRangeAsync(Guid aggregateId, DateTime since, DateTime to)
            => await Events
                .AsQueryable()
                .Where(e => e.AggregateId == aggregateId)
                .Where(e => e.ExecutionTime >= since)
                .Where(e => e.ExecutionTime <= to)
                .OrderBy(e => e.AppliedOnAggregateVersion)
                .Select(e => e.Content)
                .ToListAsync();

        public async Task<IEnumerable<IEvent>> GetAggregateEventsVersionRangeAsync(Guid aggregateId, int since, int to)
            => await Events
                .AsQueryable()
                .Where(e => e.AggregateId == aggregateId)
                .Where(e => e.AppliedOnAggregateVersion >= since)
                .Where(e => e.AppliedOnAggregateVersion < to)
                .OrderBy(e => e.AppliedOnAggregateVersion)
                .Select(e => e.Content)
                .ToListAsync();

        public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<IEvent> events)
        {
            var versionedEvent = events.FirstOrDefault(e => e is IAggregateAppliedEvent);
            if (versionedEvent != null)
                await VerifyAggregateVersionAsync(aggregateId, 
                    (versionedEvent as IAggregateAppliedEvent).AppliedOnAggregateVersion);
            await Events
                .InsertManyAsync(
                    events.Select(e => 
                        new EventEntity(aggregateId, DateTime.UtcNow, e)
                    )
                );
        }

        public async Task VerifyAggregateVersionAsync(Guid aggregateId, int version)
        {
            var currentVersion = await Events
                .AsQueryable()
                .Where(e => e.AggregateId == aggregateId)
                .Select(e => e.AppliedOnAggregateVersion)
                .MaxAsync();
            
            if (currentVersion != version)
                throw new DXGameException("incorrect_aggregate_version_detected");
        }
    }
}