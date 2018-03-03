using System;

namespace DXGame.Common.Communication
{
    public interface IAuthenticatedCommand : ICommand
    {
        Guid Requester { get; }
    }
}