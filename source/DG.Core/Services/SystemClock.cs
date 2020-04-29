namespace DG.Core.Services
{
    using System;

    public class SystemClock : ISystemClock
    {
        public DateTime Now => DateTime.Now;
    }
}
