using System;
using System.Threading.Tasks;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Messages.Abstract;
using Microsoft.AspNetCore.SignalR;

namespace DXGame.Api.Infrastructure
{
    public class SignalRBroadcaster : IBroadcaster
    {
        private IHubContext<DXGameHub> _hubContext;

        public SignalRBroadcaster(IHubContext<DXGameHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task BroadcastAsync<T>(T data) where T : IEvent
            => await _hubContext.Clients.All.SendAsync("Broadcast", nameof(T), data);

        public async Task BroadcastAsync<T>(Guid subscriptionId, T data) where T : IEvent
            => await _hubContext.Clients.Group(subscriptionId.ToString()).SendAsync("Broadcast", nameof(T), data);
    }

    public class DXGameHub : Hub
    {
        public async Task Subscribe(string subscriptionId)
            => await Groups.AddAsync(Context.ConnectionId, subscriptionId);
    }
}