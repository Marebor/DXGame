using System;
using DXGame.Messages.Abstract;

namespace DXGame.Common.Communication
{
    public interface IAuthenticatedCommand : ICommand
    {
        Guid Requester { get; }
    }
}