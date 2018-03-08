using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Common.Communication;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Api.Handlers.Playroom
{
    public class PasswordChangeFailedHandler : IEventHandler<PasswordChangeFailed>
    {
        IBroadcaster _broadcaster;

        public PasswordChangeFailedHandler(IBroadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public async Task HandleAsync(PasswordChangeFailed e) 
            => await _broadcaster.BroadcastAsync<PasswordChangeFailed>(e.RelatedCommand, e.Playroom);
    }
}