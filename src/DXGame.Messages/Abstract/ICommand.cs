using System;

namespace DXGame.Messages.Abstract
{
    public interface ICommand : IMessage
    {
        Guid CommandId { get; }
    }
}