using System;
using System.Threading.Tasks;
using DXGame.Messages.Abstract;

namespace DXGame.Common.Communication
{
    public interface IEventSubscriber
    {
        Func<IEvent, Task> OnEventReceived { get; }
    }
}