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
    public class ChangePrivacyHandler : ICommandHandler<ChangePrivacy>
    {
        private readonly IEventService _eventService;
        private readonly IHandler _handler;

        public ChangePrivacyHandler(IEventService eventService, IHandler handler)
        {
            _eventService = eventService;
            _handler = handler;
        }

        public async Task HandleAsync(ChangePrivacy command) => await _handler
            .LoadAggregate(async () =>
            {
                var playroomEvents = await _eventService.GetAggregateEventsAsync(command.Playroom);
                return Aggregate.Builder.Build<Models.Playroom>(playroomEvents);
            })
            .Validate(playroom =>
            {
                if (playroom == null || playroom.IsDeleted)
                    throw new DXGameException("playroom_with_specified_id_does_not_exist");
            })
            .Run(playroom =>
            {
                playroom.ChangePrivacy(command);
            })
            .OnSuccess(async playroom =>
            {
                await _eventService.StoreEventsAsync(playroom.Id, playroom.RecentlyAppliedEvents.ToArray());
                await _eventService.PublishEventsAsync(playroom.RecentlyAppliedEvents.ToArray());
                playroom.MarkRecentlyAppliedEventsAsConfirmed();
            })
            .OnCustomError<DXGameException>(async ex =>
            {
                await _eventService.PublishEventsAsync(new PrivacyChangeFailed(command.Playroom, ex.ErrorCode, command.CommandId));
            })
            .DoNotPropagateException()
            .OnError(async ex => 
            {
                await _eventService.PublishEventsAsync(new PrivacyChangeFailed(command.Playroom, ex.GetType().Name, command.CommandId));
            })
            .DoNotPropagateException()
            .ExecuteAsync();
    }
}