using System;
using System.Collections;
using System.Collections.Generic;
using DataGenies.InMemory;

namespace DataGenies.Core.Publishers
{
    public interface IPublisher
    {
        void Publish(MqMessage data);

        void PublishRange(IEnumerable<MqMessage> dataRange);
    }
}