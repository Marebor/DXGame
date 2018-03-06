using System.Linq;
using System.Threading.Tasks;
using DXGame.Common.Communication;
using DXGame.Common.Exceptions;
using DXGame.Common.Helpers;
using DXGame.Common.Models;
using DXGame.Common.Services;
using DXGame.Messages.Commands.Playroom;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Services.Playroom.Domain.Handlers.Commands
{
    public class StartGameHandler : ICommandHandler<StartGame>
    {
        private readonly IEventService _eventService;
        private readonly IHandler _handler;

        public StartGameHandler(IEventService eventService, IHandler handler)
        {
            _eventService = eventService;
            _handler = handler;
        }

        public async Task HandleAsync(StartGame command) => await _handler
            .LoadAggregate(async () =>
            {
                var playroomEvents = await _eventService.GetAggregateEventsAsync(command.Playroom);
                return Aggregate.Builder.Build<Models.Playroom>(playroomEvents);
            })
            .Validate(aggregate =>
            {
                if (aggregate == null || aggregate.IsDeleted)
                    throw new DXGameException("aggregate_with_specified_id_does_not_exist");
            })
            .Run(aggregate => 
            {
                (aggregate as Models.Playroom).NewGame(command.Game);
            })
            .OnSuccess(async aggregate => 
            {
                await _eventService.StoreEventsAsync(aggregate.Id, aggregate.RecentlyAppliedEvents.ToArray());
                await _eventService.PublishEventsAsync(aggregate.RecentlyAppliedEvents.ToArray());
                aggregate.MarkRecentlyAppliedEventsAsConfirmed();
            })
            .OnCustomError<DXGameException>(async ex => 
            {
                await _eventService.PublishEventsAsync(new GameStartFailed(command.Playroom, command.Game, ex.ErrorCode, null));
            })
            .OnError(async ex => 
            {
                await _eventService.PublishEventsAsync(new GameStartFailed(command.Playroom, command.Game, ex.GetType().Name, null));
            })
            .DoNotPropagateException()
            .ExecuteAsync();
    }
}