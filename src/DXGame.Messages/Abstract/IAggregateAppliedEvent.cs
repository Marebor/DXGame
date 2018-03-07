namespace DXGame.Messages.Abstract
{
    public interface IAggregateAppliedEvent : IEvent
    {
        int AppliedOnAggregateVersion { get; }
    }
}