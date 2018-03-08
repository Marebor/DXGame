using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Common.Communication;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayroomDeletionFailedHandler : IEventHandler<PlayroomDeletionFailed>
    {
        IBroadcaster _broadcaster;

        public PlayroomDeletionFailedHandler(IBroadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public async Task HandleAsync(PlayroomDeletionFailed e) 
            => await _broadcaster.BroadcastAsync<PlayroomDeletionFailed>(e.RelatedCommand, e);
    }
}