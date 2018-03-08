using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Common.Communication;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Api.Handlers.Playroom
{
    public class PasswordChangedHandler : IEventHandler<PasswordChanged>
    {
        IBroadcaster _broadcaster;

        public PasswordChangedHandler(IBroadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public async Task HandleAsync(PasswordChanged e) 
            => await _broadcaster.BroadcastAsync<PasswordChanged>(e.RelatedCommand, e.Playroom);
    }
}