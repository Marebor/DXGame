using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DXGame.ReadModel.Infrastructure.Abstract;
using DXGame.ReadModel.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DXGame.ReadModel.Infrastructure
{
    public class MongoDBProjectionRepository : IProjectionRepository
    {
        IMongoDatabase _database;
        IMongoCollection<T> Collection<T>() where T : IProjection
            => _database.GetCollection<T>(typeof(T).FullName);

        public MongoDBProjectionRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync<T>(T element) where T : IProjection
            => await Collection<T>()
                .InsertOneAsync(element);

        public async Task<IEnumerable<T>> BrowseAsync<T>() where T : IProjection
            => await Collection<T>()
                .AsQueryable()
                .ToListAsync();

        public async Task<IEnumerable<T>> BrowseAsync<T>(Func<T, bool> filter) where T : IProjection
            => await Collection<T>()
                .AsQueryable()
                .Where(p => filter(p))
                .ToListAsync();

        public async Task DeleteAsync<T>(Guid id) where T : IProjection
            => await Collection<T>()
                .DeleteOneAsync(p => p.Id == id);

        public async Task<T> GetAsync<T>(Guid id) where T : IProjection
            => await Collection<T>()
                .AsQueryable()
                .SingleAsync(p => p.Id == id);

        public async Task SaveAsync<T>(T element) where T : IProjection
            => await Collection<T>()
                .ReplaceOneAsync(p => p.Id == element.Id, element, new UpdateOptions() { IsUpsert = true });

        public async Task UpdateAsync<T>(Guid id, 
            string propertyName, object value) where T : IProjection
        {
            var update = new UpdateDefinitionBuilder<T>().Set(propertyName, value);
            var options = new UpdateOptions() { IsUpsert = true };
            await Collection<T>().UpdateOneAsync(e => e.Id == id, update, options);
        }

        public async Task UpdateAsync<T>(Guid id, 
            IEnumerable<KeyValuePair<string, object>> properties) where T : IProjection
        {
            var update = new UpdateDefinitionBuilder<T>();
            foreach (var pair in properties)
            {
                update.Set(pair.Key, pair.Value);
            }
            var options = new UpdateOptions() { IsUpsert = true };
            await Collection<T>().UpdateOneAsync(e => e.Id == id, update as UpdateDefinition<T>, options);
        }
    }
}