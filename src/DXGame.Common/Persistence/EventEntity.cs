using System;
using DXGame.Common.Communication;

namespace DXGame.Common.Persistence
{
    public class EventEntity
    {
        public Guid AggregateId { get; }
        public string Type { get; }
        public DateTime ExecutionTime { get; }
        public IEvent Content { get; }

        public EventEntity(Guid aggregateId, string type, DateTime executionTime, IEvent content)
        {
            this.AggregateId = aggregateId;
            this.Type = type;
            this.ExecutionTime = executionTime;
            this.Content = content;
        }
    }
}