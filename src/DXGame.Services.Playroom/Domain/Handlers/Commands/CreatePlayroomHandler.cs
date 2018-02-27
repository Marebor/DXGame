using System.Linq;
using System.Threading.Tasks;
using DXGame.Common.Exceptions;
using DXGame.Common.Helpers;
using DXGame.Messages.Commands;
using DXGame.Messages.Commands.Playroom;
using DXGame.Messages.Events.Playroom;
using DXGame.Common.Persistence;
using DXGame.Common.Models;

namespace DXGame.Services.Playroom.Domain.Handlers.Commands
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
                return Aggregate.Builder.Build<Models.Playroom>(playroomEvents);
            })
            .Validate(aggregate =>
            {
                if (aggregate != null)
                    throw new DXGameException("specified_id_already_in_use");
            })
            .Run(aggregate => 
            {
                aggregate = Models.Playroom.Create(command.PlayroomId, command.Name, 
                    command.IsPrivate, command.Owner, command.Password);
            })
            .OnSuccess(async aggregate => 
            {
                await _eventService.SaveEvents(aggregate.RecentlyAppliedEvents.ToArray());
                await _eventService.PublishEvents(aggregate.RecentlyAppliedEvents.ToArray());
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