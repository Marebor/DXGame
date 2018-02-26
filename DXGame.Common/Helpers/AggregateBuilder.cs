using System.Collections.Generic;
using DXGame.Common.Messages.Events;
using DXGame.Common.Models;

namespace DXGame.Common.Helpers
{
    public static class AggregateBuilder
    {
        public static T Build<T>(IEnumerable<IEvent> events) where T : Aggregate, new()
        {
            var aggregate = new T();
            foreach (var e in events) 
            {
                (aggregate as dynamic).ApplyEvent(e);
            }

            return aggregate;
        }
    }
}