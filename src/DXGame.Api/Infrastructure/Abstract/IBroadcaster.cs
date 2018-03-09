using System;
using System.Threading.Tasks;
using DXGame.Messages.Abstract;

namespace DXGame.Api.Infrastructure.Abstract
{
    public interface IBroadcaster
    {
        Task BroadcastAsync<T>(Guid? subscriptionId, object data) where T : IEvent;
    }
}