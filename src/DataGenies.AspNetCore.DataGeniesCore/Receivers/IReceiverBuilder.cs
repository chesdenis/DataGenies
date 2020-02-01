using System.Collections;
using System.Collections.Generic;

namespace DataGenies.AspNetCore.DataGeniesCore.Receivers
{
    public interface IReceiverBuilder
    {
        public IReceiver Build();
    }
}