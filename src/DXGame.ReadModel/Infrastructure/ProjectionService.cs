using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DXGame.ReadModel.Infrastructure.Abstract;
using DXGame.ReadModel.Models;

namespace DXGame.ReadModel.Infrastructure
{
    public class ProjectionService : IProjectionService
    {
        IProjectionRepository _repository;

        public ProjectionService(IProjectionRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<T>> BrowseAsync<T>() where T : IProjection
            => await _repository
                .BrowseAsync<T>();

        public async Task<IEnumerable<T>> BrowseAsync<T>(Func<T, bool> filter) where T : IProjection
            => await _repository
                .BrowseAsync(filter);

        public async Task<T> GetAsync<T>(Guid id) where T : IProjection
            => await _repository
                .GetAsync<T>(id);

        public async Task SaveAsync<T>(T element) where T : IProjection
            => await _repository
                .SaveAsync(element);

        public async Task DeleteAsync<T>(Guid id) where T : IProjection
            => await _repository
                .DeleteAsync<T>(id);

        public async Task UpdateAsync<T>(Guid id, string propertyName, object value) where T : IProjection
            => await _repository
                .UpdateAsync<T>(id, propertyName, value);

        public async Task UpdateAsync<T>(Guid id, IEnumerable<KeyValuePair<string, object>> properties) where T : IProjection
            => await _repository
                .UpdateAsync<T>(id, properties);
    }
}