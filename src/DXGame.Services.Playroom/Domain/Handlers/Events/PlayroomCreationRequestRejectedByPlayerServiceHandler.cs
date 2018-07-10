using System.Linq;
using System.Threading.Tasks;
using DXGame.Common.Communication;
using DXGame.Common.Exceptions;
using DXGame.Common.Helpers;
using DXGame.Common.Models;
using DXGame.Common.Services;
using DXGame.Messages.Events.Player;
using DXGame.Messages.Events.Playroom;
using DXGame.Services.Playroom.Domain.Models;

namespace DXGame.Services.Playroom.Domain.Handlers.Events
{
    public class PlayroomCreationRequestRejectedByPlayerServiceHandler : IEventHandler<PlayroomCreationRequestRejected>
    {
        private readonly IEventService _eventService;
        private readonly IHandler _handler;

        public PlayroomCreationRequestRejectedByPlayerServiceHandler(IEventService eventService, IHandler handler)
        {
            _eventService = eventService;
            _handler = handler;
        }

        public async Task HandleAsync(PlayroomCreationRequestRejected e) => await _handler
            .LoadAggregate(async () =>
            {
                var events = await _eventService.GetAggregateEventsAsync(e.PlayroomId);
                return Aggregate.Builder.Build<Models.Playroom>(events);
            })
            .Validate(aggregate =>
            {
                if (aggregate == null || aggregate.IsDeleted)
                    throw new DXGameException("aggregate_with_specified_id_does_not_exist");
            })
            .Run(aggregate => 
            {
                aggregate.OnPlayroomCreationRequestRejectedByPlayerService(e);
            })
            .OnSuccess(async aggregate => 
            {
                await _eventService.StoreEventsAsync(aggregate.Id, aggregate.RecentlyAppliedEvents.ToArray());
            })
            .OnError(async ex => 
            {
                await Task.CompletedTask;
            })
            .DoNotPropagateException()
            .Finally(async () =>
            {
                await _eventService.PublishEventsAsync(new PlayroomCreationFailed(e.PlayroomId, e.ReasonCode, e.RelatedCommand));
            })
            .ExecuteAsync();
    }
}