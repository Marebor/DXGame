using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Common.Communication;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayerAdditionFailedHandler : IEventHandler<PlayerAdditionFailed>
    {
        IBroadcaster _broadcaster;

        public PlayerAdditionFailedHandler(IBroadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public async Task HandleAsync(PlayerAdditionFailed e) 
            => await _broadcaster.BroadcastAsync<PlayerAdditionFailed>(e.RelatedCommand, e);
    }
}