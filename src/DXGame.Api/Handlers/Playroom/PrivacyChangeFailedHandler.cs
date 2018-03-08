using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Common.Communication;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Api.Handlers.Playroom
{
    public class PrivacyChangeFailedHandler : IEventHandler<PrivacyChangeFailed>
    {
        IBroadcaster _broadcaster;

        public PrivacyChangeFailedHandler(IBroadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public async Task HandleAsync(PrivacyChangeFailed e) 
            => await _broadcaster.BroadcastAsync<PrivacyChangeFailed>(e.RelatedCommand, e.Playroom);
    }
}