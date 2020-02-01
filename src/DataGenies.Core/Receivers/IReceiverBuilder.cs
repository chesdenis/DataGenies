using System.Collections;
using System.Collections.Generic;

namespace DataGenies.Core.Receivers
{
    public interface IReceiverBuilder
    {
        public IReceiver Build();
    }
}