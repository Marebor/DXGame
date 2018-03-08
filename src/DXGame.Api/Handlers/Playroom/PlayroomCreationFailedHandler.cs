using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Common.Communication;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayroomCreationFailedHandler : IEventHandler<PlayroomCreationFailed>
    {
        IBroadcaster _broadcaster;

        public PlayroomCreationFailedHandler(IBroadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public async Task HandleAsync(PlayroomCreationFailed e) 
            => await _broadcaster.BroadcastAsync<PlayroomCreationFailed>(e.RelatedCommand, e);
    }
}