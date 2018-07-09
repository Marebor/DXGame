using System.Threading.Tasks;
using DXGame.Common.Communication;
using DXGame.Common.Exceptions;
using DXGame.Common.Helpers;
using DXGame.Common.Models;
using DXGame.Common.Services;
using DXGame.Messages.Events.Player;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Services.Player.Domain.Handlers.Events
{
    public class PlayroomCreationRequestedHandler : IEventHandler<PlayroomCreationRequested>
    {
        private readonly IEventService _eventService;
        private readonly IHandler _handler;

        public PlayroomCreationRequestedHandler(IEventService eventService, IHandler handler)
        {
            _eventService = eventService;
            _handler = handler;
        }

        public async Task HandleAsync(PlayroomCreationRequested e) => await _handler
            .LoadAggregate(async () =>
            {
                var events = await _eventService.GetAggregateEventsAsync(e.Owner);
                return Aggregate.Builder.Build<Models.Player>(events);
            })
            .Validate(aggregate =>
            {
                if (aggregate == null || aggregate.IsDeleted)
                    throw new DXGameException("aggregate_with_specified_id_does_not_exist");
            })
            .OnSuccess(async aggregate => 
            {
                await _eventService.PublishEventsAsync(new PlayroomCreationRequestAccepted(e.Id, e.RelatedCommand));
            })
            .OnCustomError<DXGameException>(async ex => 
            {
                await _eventService.PublishEventsAsync(new PlayroomCreationRequestRejected(e.Id, ex.ErrorCode, e.RelatedCommand));
            })
            .DoNotPropagateException()
            .OnError(async ex => 
            {
                await _eventService.PublishEventsAsync(new PlayroomCreationRequestRejected(e.Id, ex.GetType().Name, e.RelatedCommand));
            })
            .DoNotPropagateException()
            .ExecuteAsync();
    }
}