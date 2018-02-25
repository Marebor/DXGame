using DXGame.Common.Models;

namespace DXGame.Common.Helpers
{
    public interface IAggregateBuilder<T> where T : IEntity
    {
        T Build(IEnumerable<IEvent> events);
    }
}