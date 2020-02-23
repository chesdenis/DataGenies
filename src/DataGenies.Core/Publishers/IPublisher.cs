using System;
using System.Collections;
using System.Collections.Generic;
using DataGenies.Core.Models;

namespace DataGenies.Core.Publishers
{
    public interface IPublisher
    {
        void Publish(string exchange, MqMessage data);
        
        void PublishRange(string exchange, IEnumerable<MqMessage> dataRange);
    }
}