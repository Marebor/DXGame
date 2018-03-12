using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DXGame.Api.Infrastructure.Abstract
{
    public interface ICache
    {
        Task<IEnumerable<T>> BrowseAsync<T>();
        Task<T> GetAsync<T>(Guid key);
        Task SetAsync<T>(Guid key, T value);
        Task RemoveAsync<T>(Guid key);
    }
}