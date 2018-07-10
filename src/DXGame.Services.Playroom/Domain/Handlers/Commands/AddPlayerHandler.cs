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
    public class AddPlayerHandler : ICommandHandler<AddPlayer>
    {
        private readonly IEventService _eventService;
        private readonly IHandler _handler;

        public AddPlayerHandler(IEventService eventService, IHandler handler)
        {
            _eventService = eventService;
            _handler = handler;
        }
        public async Task HandleAsync(AddPlayer command) => await _handler
            .LoadAggregate(async () =>
            {
                var events = await _eventService.GetAggregateEventsAsync(command.Playroom);
                return Aggregate.Builder.Build<Models.Playroom>(events);
            })
            .Validate(aggregate =>
            {
                if (aggregate == null || aggregate.IsDeleted)
                    throw new DXGameException("aggregate_with_specified_id_does_not_exist");
                if (command.Password != aggregate.Password)
                    throw new DXGameException("invalid_password");
                if (aggregate.Players.Any(p => p == command.Player))
                    throw new DXGameException("playroom_already_contains_specified_player");
            })
            .OnSuccess(async playroom =>
            {
                await _eventService.PublishEventsAsync(new PlayerAdditionRequested(command.Playroom, command.Player, command.CommandId));
            })
            .OnCustomError<DXGameException>(async ex =>
            {
                await _eventService.PublishEventsAsync(new PlayerAdditionFailed(command.Playroom, command.Player, ex.ErrorCode, command.CommandId));
            })
            .DoNotPropagateException()
            .OnError(async ex => 
            {
                await _eventService.PublishEventsAsync(new PlayerAdditionFailed(command.Playroom, command.Player, ex.GetType().Name, command.CommandId));
            })
            .DoNotPropagateException()
            .ExecuteAsync();
    }
}