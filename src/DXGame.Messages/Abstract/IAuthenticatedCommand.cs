using System;

namespace DXGame.Messages.Abstract
{
    public interface IAuthenticatedCommand : ICommand
    {
        Guid Requester { get; }
    }
}