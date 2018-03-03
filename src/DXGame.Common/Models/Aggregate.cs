using System;
using System.Collections.Generic;
using System.Linq;
using DXGame.Common.Exceptions;
using DXGame.Common.Communication;

namespace DXGame.Common.Models
{
    public abstract class Aggregate
    {
        public Guid Id { get; protected set; }

        public bool IsDeleted { get; protected set; }

        private ISet<IEvent> _recentlyAppliedEvents = new HashSet<IEvent>();
        public IEnumerable<IEvent> RecentlyAppliedEvents 
        { 
            get { return _recentlyAppliedEvents.ToArray(); }
            protected set { _recentlyAppliedEvents = new HashSet<IEvent>(value); }
        }
        private Dictionary<Type, Action<IEvent>> _eventAppliers =  new Dictionary<Type, Action<IEvent>>();

        protected Aggregate() 
        {
            RegisterAppliers();
        }

        protected abstract void RegisterAppliers();

        protected void RegisterApplier<T>(Action<T> applier) where T : IEvent
        {
            _eventAppliers.Add(typeof(T), (x) => applier((T)x));
        }

        public void MarkRecentlyAppliedEventsAsConfirmed()
        {
            _recentlyAppliedEvents.Clear();
        }

        protected void ApplyEvent(IEvent e)
        {
            var type = e.GetType();
            if (!_eventAppliers.ContainsKey(type))
            {
                throw new DXGameException("could_not_find_event_applier_for_aggregate");
            }
            _eventAppliers[type](e);
            _recentlyAppliedEvents.Add(e);
        }

        public static class Builder
        {
            public static T Build<T>(IEnumerable<IEvent> events) where T : Aggregate, new()
            {
                if (events == null || events.Count() == 0)
                    return null;
                    
                var aggregate = new T();
                foreach (var e in events) 
                {
                    aggregate.ApplyEvent(e);
                }
                aggregate.MarkRecentlyAppliedEventsAsConfirmed();

                return aggregate;
            }
        }
    }
}