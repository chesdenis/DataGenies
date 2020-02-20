using System;
using System.Collections;
using System.Collections.Generic;
using DataGenies.Core.Models;

namespace DataGenies.Core.Publishers
{
    public interface IPublisher
    {
        void Publish(MqMessage data);

        void PublishRange(IEnumerable<MqMessage> dataRange);
    }
}