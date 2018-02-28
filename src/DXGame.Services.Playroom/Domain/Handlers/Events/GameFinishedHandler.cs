using System.Linq;
using System.Threading.Tasks;
using DXGame.Common.Exceptions;
using DXGame.Common.Helpers;
using DXGame.Common.Models;
using DXGame.Messages.Events;
using DXGame.Messages.Events.Game;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Services.Playroom.Domain.Handlers.Events
{
    public class GameFinishedHandler : IEventHandler<GameFinished>
    {
        private readonly IEventService _eventService;
        private readonly IHandler _handler;

        public GameFinishedHandler(IEventService eventService, IHandler handler)
        {
            _eventService = eventService;
            _handler = handler;
        }

        public async Task HandleAsync(GameFinished @event) => await _handler
            .LoadAggregate(async () =>
            {
                var playroomEvents = await _eventService.GetAggregateEventsAsync(@event.Playroom);
                return Aggregate.Builder.Build<Models.Playroom>(playroomEvents);
            })
            .Validate(aggregate =>
            {
                if (aggregate == null || aggregate.IsDeleted)
                    throw new DXGameException("aggregate_with_specified_id_does_not_exist");
            })
            .Run(aggregate =>
            {
                (aggregate as Models.Playroom).OnGameStartRequestAccepted(@event.Game);
            })
            .OnSuccess(async aggregate =>
            {
                await _eventService.SaveEvents(aggregate.RecentlyAppliedEvents.ToArray());
                await _eventService.PublishEvents(aggregate.RecentlyAppliedEvents.ToArray());
                aggregate.MarkRecentlyAppliedEventsAsConfirmed();
            })
            .OnCustomError<DXGameException>(async ex =>
            {
                await _eventService.PublishEvents(new GameStartFailed(@event.Playroom, @event.Game, ex.ErrorCode));
            })
            .OnError(async ex => 
            {
                await _eventService.PublishEvents(new GameStartFailed(@event.Playroom, @event.Game, ex.GetType().Name));
            })
            .DoNotPropagateException()
            .ExecuteAsync();
    }
}