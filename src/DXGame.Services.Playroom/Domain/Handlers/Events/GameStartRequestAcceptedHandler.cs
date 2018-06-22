using System.Linq;
using System.Threading.Tasks;
using DXGame.Common.Communication;
using DXGame.Common.Exceptions;
using DXGame.Common.Helpers;
using DXGame.Common.Models;
using DXGame.Common.Services;
using DXGame.Messages.Events.Game;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Services.Playroom.Domain.Handlers.Events
{
    public class GameStartRequestAcceptedHandler : IEventHandler<GameStartRequestAccepted>
    {
        private readonly IEventService _eventService;
        private readonly IHandler _handler;

        public GameStartRequestAcceptedHandler(IEventService eventService, IHandler handler)
        {
            _eventService = eventService;
            _handler = handler;
        }
        public async Task HandleAsync(GameStartRequestAccepted e) => await _handler
            .LoadAggregate(async () =>
            {
                var playroomEvents = await _eventService.GetAggregateEventsAsync(e.Playroom);
                return Aggregate.Builder.Build<Models.Playroom>(playroomEvents);
            })
            .Validate(playroom =>
            {
                if (playroom == null || playroom.IsDeleted)
                    throw new DXGameException("playroom_with_specified_id_does_not_exist");
            })
            .Run(playroom =>
            {
                playroom.OnGameStartRequestAccepted(e);
            })
            .OnSuccess(async playroom =>
            {
                await _eventService.StoreEventsAsync(playroom.Id, playroom.RecentlyAppliedEvents.ToArray());
                await _eventService.PublishEventsAsync(playroom.RecentlyAppliedEvents.ToArray());
                playroom.MarkRecentlyAppliedEventsAsConfirmed();
            })
            .OnCustomError<DXGameException>(async ex =>
            {
                await _eventService.PublishEventsAsync(new GameStartFailed(e.Playroom, e.Game, ex.ErrorCode, e.RelatedCommand));
            })
            .DoNotPropagateException()
            .OnError(async ex => 
            {
                await _eventService.PublishEventsAsync(new GameStartFailed(e.Playroom, e.Game, ex.GetType().Name, e.RelatedCommand));
            })
            .DoNotPropagateException()
            .ExecuteAsync();
    }
}