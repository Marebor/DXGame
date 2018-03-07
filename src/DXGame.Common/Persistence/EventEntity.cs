using System;
using DXGame.Messages.Abstract;

namespace DXGame.Common.Persistence
{
    public class EventEntity
    {
        public Guid AggregateId { get; }
        public int AppliedOnAggregateVersion { get; }
        public string Type { get; }
        public DateTime ExecutionTime { get; }
        public IEvent Content { get; }

        public EventEntity(Guid aggregateId, DateTime executionTime, IEvent content)
        {
            this.AggregateId = aggregateId;
            this.AppliedOnAggregateVersion = content is IAggregateAppliedEvent ? 
                (content as IAggregateAppliedEvent).AppliedOnAggregateVersion : -1;
            this.Type = content.GetType().ToString();
            this.ExecutionTime = executionTime;
            this.Content = content;
        }
    }
}