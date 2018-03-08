using System;
using System.Threading.Tasks;

namespace DXGame.Api.Models
{
    public interface ICache
    {
        Task<T> GetAsync<T>(Guid key);
        Task SetAsync<T>(Guid key, T value);
    }
}