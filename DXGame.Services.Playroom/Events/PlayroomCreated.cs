using DXGame.Common.Messages.Events;

namespace DXGame.Services.Playroom.Events
{
    internal class PlayroomCreated : IEvent
    {
        internal DXGame.Services.Playroom.Domain.Models.Playroom Playroom { get; }

        internal PlayroomCreated(DXGame.Services.Playroom.Domain.Models.Playroom playroom) 
        {
            Playroom = playroom;
        }
    }
}