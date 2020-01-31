using System.Collections;
using System.Collections.Generic;

namespace DataGenies.AspNetCore.DataGeniesCore.Receivers
{
    public abstract class ReceiverBuilder
    {
        public abstract IReceiver Build();
    }
}