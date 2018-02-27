using System;
using System.Collections.Generic;
using System.Linq;
using DXGame.Common.Messages.Events;

namespace DXGame.Common.Models
{
    public abstract class Aggregate
    {
        public Guid Id { get; protected set; }

        public bool IsDeleted { get; protected set; }

        private ISet<IEvent> _recentlyAppliedEvents = new HashSet<IEvent>();
        public IEvent[] RecentlyAppliedEvents 
        { 
            get { return _recentlyAppliedEvents.ToArray(); }
            protected set { _recentlyAppliedEvents = new HashSet<IEvent>(value); }
        }

        public void MarkRecentlyAppliedEventsAsConfirmed()
        {
            _recentlyAppliedEvents.Clear();
        }

        protected void AddRecentlyAppliedEvent(IEvent e) 
        {
            _recentlyAppliedEvents.Add(e);
        }
    }
}