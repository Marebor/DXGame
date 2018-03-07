using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Api.Models.Dto;
using DXGame.Common.Communication;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Api.Handlers.Playroom
{
    public class PlayerLeftHandler : IEventHandler<PlayerLeft>
    {
        IBroadcaster _broadcaster;
        ICache _cache;

        public PlayerLeftHandler(IBroadcaster broadcaster, ICache cache)
        {
            _broadcaster = broadcaster;
            _cache = cache;
        }
        public async Task HandleAsync(PlayerLeft e)
        {
            var playroom = _cache.Get<PlayroomDto>(e.Playroom);
            playroom.Players.Remove(e.Player);
            _cache.Set(playroom.Id, playroom);
            await _broadcaster.BroadcastAsync<PlayerJoined>(e);
        }
    }
}