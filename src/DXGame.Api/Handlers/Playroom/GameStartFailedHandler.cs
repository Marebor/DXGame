using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Api.Models.Dto;
using DXGame.Common.Communication;
using DXGame.Common.Helpers;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Api.Handlers.Playroom
{
    public class GameStartFailedHandler : IEventHandler<GameStartFailed>
    {
        IBroadcaster _broadcaster;

        public GameStartFailedHandler(IBroadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public async Task HandleAsync(GameStartFailed e) 
            => await _broadcaster.BroadcastAsync<GameStartFailed>(e.RelatedCommand, e);
    }
}