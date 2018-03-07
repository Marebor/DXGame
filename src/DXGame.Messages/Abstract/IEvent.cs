namespace DXGame.Messages.Abstract
{
    public interface IEvent : IMessage
    {
        int? AppliedOnAggregateVersion { get; }
    }
}