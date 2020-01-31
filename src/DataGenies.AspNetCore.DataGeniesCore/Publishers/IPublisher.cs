using System;
using System.Collections;
using System.Collections.Generic;

namespace DataGenies.AspNetCore.DataGeniesCore.Publishers
{
    public interface IPublisher
    {
        void Publish(byte[] data);

        void Publish(byte[] data, string routingKey);

        void Publish(byte[] data, IEnumerable<string> routingKeys);
        
        void PublishRange(IEnumerable<byte[]> dataRange);

        void PublishRange(IEnumerable<byte[]> dataRange, string routingKey);

        void PublishTuples(IEnumerable<Tuple<byte[], string>> tuples);
    }
}