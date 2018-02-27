using System.Linq;
using System.Threading.Tasks;
using DXGame.Common.Exceptions;
using DXGame.Common.Helpers;
using DXGame.Common.Messages.Commands;
using DXGame.Common.Messages.Commands.Playroom;
using DXGame.Common.Messages.Events.Playroom;
using DXGame.Common.Persistence;

namespace DXGame.Services.Playroom.Domain.Handlers
{
    public class CreatePlayroomHandler : ICommandHandler<CreatePlayroom>
    {
        private readonly IEventService _eventService;
        private readonly IHandler _handler;

        public CreatePlayroomHandler(IEventService eventService, IHandler handler)
        {
            _eventService = eventService;
            _handler = handler;
        }

        public async Task HandleAsync(CreatePlayroom command) => await _handler
            .LoadAggregate(async () =>
            {
                var playroomEvents = await _eventService.GetAggregateEventsAsync(command.PlayroomId);
                return AggregateBuilder.Build<Models.Playroom>(playroomEvents);
            })
            .Validate(aggregate =>
            {
                if (aggregate != null)
                    throw new DXGameException("specified_id_already_in_use");
            })
            .Run(aggregate => 
            {
                aggregate = Models.Playroom.Create(command.PlayroomId, command.Name, 
                    command.IsPrivate, command.OwnerPlayerId, command.Password);
            })
            .OnSuccess(async aggregate => 
            {
                await _eventService.SaveEvents(aggregate.RecentlyAppliedEvents);
                await _eventService.PublishEvents(aggregate.RecentlyAppliedEvents);
                aggregate.MarkRecentlyAppliedEventsAsConfirmed();
            })
            .OnCustomError<DXGameException>(async ex => 
            {
                await _eventService.PublishEvents(new PlayroomCreationFailed(command.PlayroomId, ex.ErrorCode));
            })
            .OnError(async ex => 
            {
                await _eventService.PublishEvents(new PlayroomCreationFailed(command.PlayroomId, ex.GetType().Name));
            })
            .DoNotPropagateException()
            .ExecuteAsync();
    }
}