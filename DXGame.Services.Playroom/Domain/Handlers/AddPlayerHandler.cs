using System.Threading.Tasks;
using DXGame.Common.Exceptions;
using DXGame.Common.Helpers;
using DXGame.Common.Messages.Commands;
using DXGame.Common.Messages.Commands.Playroom;
using DXGame.Common.Messages.Events.Playroom;

namespace DXGame.Services.Playroom.Domain.Handlers
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
            .Run(async () =>
            {
                var playroomEvents = await _eventService.GetAggregateEventsAsync(command.Playroom);
                var playroom = AggregateBuilder.Build<Models.Playroom>(playroomEvents);
                playroom.AddPlayer(command.Player);
            })
            .OnSuccess(async () =>
            {
                var @event = new PlayerJoined(command.Playroom, command.Player);
                await _eventService.SaveEvents(@event);
                await _eventService.PublishEvents(@event);
            })
            .OnCustomError<DXGameException>(async ex =>
            {
                await _eventService.PublishEvents(new PlayerAdditionFailed(command.Playroom, command.Player, ex.ErrorCode));
            })
            .OnError(async ex => 
            {
                await _eventService.PublishEvents(new PlayerAdditionFailed(command.Playroom, command.Player, ex.GetType().Name));
            })
            .DoNotPropagateException()
            .ExecuteAsync();
    }
}