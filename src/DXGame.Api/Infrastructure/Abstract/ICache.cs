using System;
using System.Threading.Tasks;

namespace DXGame.Api.Infrastructure.Abstract
{
    public interface ICache
    {
        Task<T> GetAsync<T>(object key);
        Task SetAsync<T>(object key, T value);
    }
}