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
    public class GameStartRequestRejectedHandler : IEventHandler<GameStartRequestRejected>
    {
        private readonly IEventService _eventService;
        private readonly IHandler _handler;

        public GameStartRequestRejectedHandler(IEventService eventService, IHandler handler)
        {
            _eventService = eventService;
            _handler = handler;
        }
        public async Task HandleAsync(GameStartRequestRejected e) => await _handler
            .LoadAggregate(async () =>
            {
                var playroomEvents = await _eventService.GetAggregateEventsAsync(e.Playroom);
                return Aggregate.Builder.Build<Models.Playroom>(playroomEvents);
            })
            .Validate(aggregate =>
            {
                if (aggregate == null || aggregate.IsDeleted)
                    throw new DXGameException("aggregate_with_specified_id_does_not_exist");
            })
            .Run(aggregate =>
            {
                (aggregate as Models.Playroom).OnGameStartRequestRejected(e);
            })
            .OnSuccess(async aggregate =>
            {
                await _eventService.StoreEventsAsync(aggregate.Id, aggregate.RecentlyAppliedEvents.ToArray());
                await _eventService.PublishEventsAsync(aggregate.RecentlyAppliedEvents.ToArray());
                aggregate.MarkRecentlyAppliedEventsAsConfirmed();
            })
            .OnCustomError<DXGameException>(async ex =>
            {
                await _eventService.PublishEventsAsync(new GameStartFailed(e.Playroom, e.Game, ex.ErrorCode, e.RelatedCommand));
            })
            .OnError(async ex => 
            {
                await _eventService.PublishEventsAsync(new GameStartFailed(e.Playroom, e.Game, ex.GetType().Name, e.RelatedCommand));
            })
            .DoNotPropagateException()
            .ExecuteAsync();
    }
}