using System;
using System.Threading.Tasks;
using DXGame.Common.Communication;
using DXGame.Messages.Abstract;

namespace DXGame.Api.Models
{
    public class EventSubscriber : IEventSubscriber
    {
        IBroadcaster _brodcaster;

        public EventSubscriber(IBroadcaster broadcaster)
        {
            _brodcaster = broadcaster;
        }

        public Func<IEvent, Task> OnEventReceived => _brodcaster.BroadcastAsync;
    }
}