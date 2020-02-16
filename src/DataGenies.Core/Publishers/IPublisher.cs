using System;
using System.Collections;
using System.Collections.Generic;
using DataGenies.InMemory;

namespace DataGenies.Core.Publishers
{
    public interface IPublisher
    {
        void Publish(MqMessage data);

        void Publish(MqMessage data, string routingKey);

        void Publish(MqMessage data, IEnumerable<string> routingKeys);
        
        void PublishRange(IEnumerable<MqMessage> dataRange);

        void PublishRange(IEnumerable<MqMessage> dataRange, string routingKey);

        void PublishTuples(IEnumerable<Tuple<MqMessage, string>> tuples);
    }
}