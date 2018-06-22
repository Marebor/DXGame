using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DXGame.ReadModel.Models;

namespace DXGame.ReadModel.Infrastructure.Abstract
{
    public interface IProjectionService
    {
        Task<IEnumerable<T>> BrowseAsync<T>() where T : IProjection;
        Task<IEnumerable<T>> BrowseAsync<T>(Func<T, bool> filter) where T : IProjection;
        Task<T> GetAsync<T>(Guid id) where T : IProjection;
        Task SaveAsync<T>(T element) where T : IProjection;
        Task DeleteAsync<T>(Guid id) where T : IProjection;
        Task UpdateAsync<T>(Guid id, string propertyName, object value) where T : IProjection;
        Task UpdateAsync<T>(Guid id, IEnumerable<KeyValuePair<string, object>> properties) where T : IProjection;
    }
}