namespace DG.Core.Services
{
    using System;

    public interface ISystemClock
    {
        DateTime Now { get; }
    }
}
