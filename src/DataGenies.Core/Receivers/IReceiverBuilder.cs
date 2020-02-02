using System.Collections;
using System.Collections.Generic;

namespace DataGenies.Core.Receivers
{
    public interface IReceiverBuilder
    {
        public IReceiverBuilder WithQueue(string queueName);
        
        public IReceiver Build();
    }
}