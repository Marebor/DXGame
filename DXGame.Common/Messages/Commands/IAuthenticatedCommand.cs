using System;

namespace DXGame.Common.Messages.Commands
{
    public interface IAuthenticatedCommand : ICommand
    {
        Guid Requester { get; }
    }
}