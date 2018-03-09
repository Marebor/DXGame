using System;
using System.Threading.Tasks;

namespace DXGame.Api.Infrastructure.Abstract
{
    public interface ICache
    {
        Task<T> GetAsync<T>(Guid key);
        Task SetAsync<T>(Guid key, T value);
    }
}