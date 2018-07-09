using System.Linq;
using System.Threading.Tasks;
using DXGame.Common.Communication;
using DXGame.Common.Exceptions;
using DXGame.Common.Helpers;
using DXGame.Common.Models;
using DXGame.Common.Services;
using DXGame.Messages.Commands.Player;
using DXGame.Messages.Events.Player;

namespace DXGame.Services.Player.Domain.Handlers.Commands
{
    public class CreatePlayerHandler : ICommandHandler<CreatePlayer>
    {
        private readonly IEventService _eventService;
        private readonly IHandler _handler;

        public CreatePlayerHandler(IEventService eventService, IHandler handler)
        {
            _eventService = eventService;
            _handler = handler;
        }

        public async Task HandleAsync(CreatePlayer command) => await _handler
            .LoadAggregate(async () =>
            {
                var events = await _eventService.GetAggregateEventsAsync(command.PlayerId);
                return Aggregate.Builder.Build<Models.Player>(events);
            })
            .Validate(aggregate =>
            {
                if (aggregate != null)
                    throw new DXGameException("specified_id_already_in_use");
            })
            .Run((ref Models.Player aggregate) => 
            {
                aggregate = Models.Player.Create(command);
            })
            .OnSuccess(async aggregate => 
            {
                await _eventService.StoreEventsAsync(aggregate.Id, aggregate.RecentlyAppliedEvents.ToArray());
                await _eventService.PublishEventsAsync(aggregate.RecentlyAppliedEvents.ToArray());
                aggregate.MarkRecentlyAppliedEventsAsConfirmed();
            })
            .OnCustomError<DXGameException>(async ex => 
            {
                await _eventService.PublishEventsAsync(new PlayerCreationFailed(command.PlayerId, ex.ErrorCode, command.CommandId));
            })
            .DoNotPropagateException()
            .OnError(async ex => 
            {
                await _eventService.PublishEventsAsync(new PlayerCreationFailed(command.PlayerId, ex.GetType().Name, command.CommandId));
            })
            .DoNotPropagateException()
            .ExecuteAsync();
    }
}