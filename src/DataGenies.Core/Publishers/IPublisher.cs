using System;
using System.Collections;
using System.Collections.Generic;
using DataGenies.Core.Models;

namespace DataGenies.Core.Publishers
{
    public interface IPublisher
    {
        void Publish(MqMessage data);

        void Publish(string exchange, MqMessage data);

        void PublishRange(IEnumerable<MqMessage> dataRange);

        void PublishRange(string exchange, IEnumerable<MqMessage> dataRange);
    }
}