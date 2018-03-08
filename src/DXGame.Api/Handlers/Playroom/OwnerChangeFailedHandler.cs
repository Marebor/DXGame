using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Api.Models.Dto;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Api.Handlers.Playroom
{
    public class OwnerChangeFailedHandler : IEventHandler<OwnerChangeFailed>
    {
        IBroadcaster _broadcaster;

        public OwnerChangeFailedHandler(IBroadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public async Task HandleAsync(OwnerChangeFailed e) 
            => await _broadcaster.BroadcastAsync<OwnerChangeFailed>(e.RelatedCommand, e);
    }
}