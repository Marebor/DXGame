using System.Linq;
using System.Threading.Tasks;
using DXGame.Common.Exceptions;
using DXGame.Common.Helpers;
using DXGame.Common.Models;
using DXGame.Messages.Commands;
using DXGame.Messages.Commands.Playroom;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Services.Playroom.Domain.Handlers.Commands
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
                (aggregate as Models.Playroom).ChangePassword(command.OldPassword, command.NewPassword, command.Requester);
            })
            .OnSuccess(async aggregate =>
            {
                await _eventService.SaveEvents(aggregate.RecentlyAppliedEvents.ToArray());
                await _eventService.PublishEvents(aggregate.RecentlyAppliedEvents.ToArray());
                aggregate.MarkRecentlyAppliedEventsAsConfirmed();
            })
            .OnCustomError<DXGameException>(async ex =>
            {
                await _eventService.PublishEvents(new PasswordChangeFailed(command.Playroom, ex.ErrorCode));
            })
            .OnError(async ex => 
            {
                await _eventService.PublishEvents(new PasswordChangeFailed(command.Playroom, ex.GetType().Name));
            })
            .DoNotPropagateException()
            .ExecuteAsync();
    }
}