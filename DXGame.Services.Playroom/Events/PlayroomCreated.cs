using DXGame.Common.Messages.Events;

namespace DXGame.Services.Playroom.Events
{
    public class PlayroomCreated : IEvent
    {
        public DXGame.Services.Playroom.Domain.Models.Playroom Playroom { get; }

        public PlayroomCreated(DXGame.Services.Playroom.Domain.Models.Playroom playroom) 
        {
            Playroom = playroom;
        }
    }
}