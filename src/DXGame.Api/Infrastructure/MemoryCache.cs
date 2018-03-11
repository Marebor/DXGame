using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using Microsoft.Extensions.Caching.Memory;

namespace DXGame.Api.Infrastructure
{
    public class MemoryCache : ICache
    {
        IMemoryCache _cache;

        public MemoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        private IDictionary<Guid, T> GetCollection<T>() 
            => _cache.GetOrCreate<IDictionary<Guid, T>>(typeof(T).Name, entry => new Dictionary<Guid, T>());

        private void UpdateCollection<T>(IDictionary<Guid, T> collection)
            => _cache.Set<IDictionary<Guid, T>>(typeof(T).Name, collection);
        
        public Task<IEnumerable<T>> BrowseAsync<T>()
            => Task.FromResult(GetCollection<T>().Values as IEnumerable<T>);

        public Task<T> GetAsync<T>(Guid key) 
            => Task.FromResult(GetCollection<T>()[key]);

        public Task SetAsync<T>(Guid key, T value)
        {
            var collection = GetCollection<T>();
            if (collection.ContainsKey(key))
            {
                collection[key] = value;
            }
            else
            {
                collection.Add(key, value);
            }
            UpdateCollection(collection);
            return Task.CompletedTask;
        }
    }
}