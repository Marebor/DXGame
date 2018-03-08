using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Common.Communication;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayerRemovalFailedHandler : IEventHandler<PlayerRemovalFailed>
    {
        IBroadcaster _broadcaster;

        public PlayerRemovalFailedHandler(IBroadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public async Task HandleAsync(PlayerRemovalFailed e) 
            => await _broadcaster.BroadcastAsync<PlayerRemovalFailed>(e.RelatedCommand, e);
    }
}