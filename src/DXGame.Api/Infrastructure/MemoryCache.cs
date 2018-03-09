using System;
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

        public Task<T> GetAsync<T>(object key) 
            => Task.FromResult(_cache.Get<T>(key));

        public Task SetAsync<T>(object key, T value)
            => Task.FromResult(_cache.Set<T>(key, value));
    }
}