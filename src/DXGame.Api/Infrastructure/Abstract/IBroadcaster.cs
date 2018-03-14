using System;
using System.Threading.Tasks;
using DXGame.Messages.Abstract;

namespace DXGame.Api.Infrastructure.Abstract
{
    public interface IBroadcaster
    {
        Task BroadcastAsync<T>(T data) where T : IEvent;
        Task BroadcastAsync<T>(Guid subscriptionId, T data) where T : IEvent;
    }
}