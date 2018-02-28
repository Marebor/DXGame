using System;

namespace DXGame.Common.Services
{
    public interface ITimeProvider
    {
        DateTime GetCurrentTime();
    }
}