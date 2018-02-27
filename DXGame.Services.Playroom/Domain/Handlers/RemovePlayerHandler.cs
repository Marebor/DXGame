using System.Threading.Tasks;
using DXGame.Common.Exceptions;
using DXGame.Common.Helpers;
using DXGame.Common.Messages.Commands;
using DXGame.Common.Messages.Commands.Playroom;
using DXGame.Common.Messages.Events.Playroom;

namespace DXGame.Services.Playroom.Domain.Handlers
{
    public class RemovePlayerHandler : ICommandHandler<RemovePlayer>
    {
        private readonly IEventService _eventService;
        private readonly IHandler _handler;

        public RemovePlayerHandler(IEventService eventService, IHandler handler)
        {
            _eventService = eventService;
            _handler = handler;
        }

        public async Task HandleAsync(RemovePlayer command) => await _handler
            .LoadAggregate(async () =>
            {
                var playroomEvents = await _eventService.GetAggregateEventsAsync(command.Playroom);
                return AggregateBuilder.Build<Models.Playroom>(playroomEvents);
            })
            .Validate(aggregate =>
            {
                if (aggregate == null)
                    throw new DXGameException("aggregate_with_specified_id_does_not_exist");
            })
            .Run(aggregate => 
            {
                (aggregate as Models.Playroom).RemovePlayer(command.Player);
            })
            .OnSuccess(async aggregate => 
            {
                await _eventService.SaveEvents(aggregate.RecentlyAppliedEvents);
                await _eventService.PublishEvents(aggregate.RecentlyAppliedEvents);
                aggregate.MarkRecentlyAppliedEventsAsConfirmed();
            })
            .OnCustomError<DXGameException>(async ex => 
            {
                await _eventService.PublishEvents(new PlayerRemovalFailed(command.Playroom, command.Player, ex.ErrorCode));
            })
            .OnError(async ex => 
            {
                await _eventService.PublishEvents(new PlayerRemovalFailed(command.Playroom, command.Player, ex.GetType().Name));
            })
            .DoNotPropagateException()
            .ExecuteAsync();
    }
}