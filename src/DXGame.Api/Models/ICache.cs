using System;

namespace DXGame.Api.Models
{
    public interface ICache
    {
        T Get<T>(Guid key);
        void Set<T>(Guid key, T value);
    }
}