namespace DXGame.Common.Communication
{
    public interface IEvent : IMessage
    {
        int? AppliedOnAggregateVersion { get; }
    }
}