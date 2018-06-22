using System;
using DXGame.Messages.Abstract;

namespace DXGame.Common.Persistence
{
    public class EventEntity
    {
        public Guid AggregateId { get; protected set; }
        public int AppliedOnAggregateVersion { get; protected set; }
        public Guid? RelatedCommand { get; protected set; }
        public string Type { get; protected set; }
        public DateTime ExecutionTime { get; protected set; }
        public IEvent Content { get; protected set; }

        public EventEntity(Guid aggregateId, DateTime executionTime, IEvent content)
        {
            this.AggregateId = aggregateId;
            this.AppliedOnAggregateVersion = content is IAggregateAppliedEvent ? 
                (content as IAggregateAppliedEvent).AppliedOnAggregateVersion : -1;
            this.RelatedCommand = content is ICommandRelatedEvent ?
                (content as ICommandRelatedEvent).RelatedCommand as Guid? : null;
            this.Type = content.GetType().ToString();
            this.ExecutionTime = executionTime;
            this.Content = content;
        }
    }
}