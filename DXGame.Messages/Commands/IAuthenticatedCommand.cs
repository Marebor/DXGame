using System;

namespace DXGame.Messages.Commands
{
    public interface IAuthenticatedCommand : ICommand
    {
        Guid Requester { get; }
    }
}