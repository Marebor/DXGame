using System.Collections.Generic;
using System.Linq;
using DXGame.Common.Messages.Events;
using DXGame.Common.Models;

namespace DXGame.Common.Helpers
{
    public static class AggregateBuilder
    {
        public static T Build<T>(IEnumerable<IEvent> events) where T : Aggregate, new()
        {
            if (events == null || events.Count() == 0)
                return null;
                
            dynamic aggregate = new T();
            foreach (var e in events) 
            {
                aggregate.ApplyEvent(e);
            }
            aggregate.MarkRecentlyAppliedEventsAsConfirmed();

            return aggregate;
        }
    }
}