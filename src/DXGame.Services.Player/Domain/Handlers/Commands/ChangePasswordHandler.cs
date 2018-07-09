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
    public class ChangePasswordHandler : ICommandHandler<ChangePassword>
    {
        private readonly IEventService _eventService;
        private readonly IHandler _handler;

        public ChangePasswordHandler(IEventService eventService, IHandler handler)
        {
            _eventService = eventService;
            _handler = handler;
        }

        public async Task HandleAsync(ChangePassword command) => await _handler
            .LoadAggregate(async () =>
            {
                var events = await _eventService.GetAggregateEventsAsync(command.Player);
                return Aggregate.Builder.Build<Models.Player>(events);
            })
            .Validate(aggregate =>
            {
                if (aggregate == null || aggregate.IsDeleted)
                    throw new DXGameException("playroom_with_specified_id_does_not_exist");
            })
            .Run(aggregate =>
            {
                aggregate.ChangePassword(command);
            })
            .OnSuccess(async aggregate =>
            {
                await _eventService.StoreEventsAsync(aggregate.Id, aggregate.RecentlyAppliedEvents.ToArray());
                await _eventService.PublishEventsAsync(aggregate.RecentlyAppliedEvents.ToArray());
                aggregate.MarkRecentlyAppliedEventsAsConfirmed();
            })
            .OnCustomError<DXGameException>(async ex =>
            {
                await _eventService.PublishEventsAsync(new PasswordChangeFailed(command.Player, ex.ErrorCode, command.CommandId));
            })
            .DoNotPropagateException()
            .OnError(async ex => 
            {
                await _eventService.PublishEventsAsync(new PasswordChangeFailed(command.Player, ex.GetType().Name, command.CommandId));
            })
            .DoNotPropagateException()
            .ExecuteAsync();
    }
}