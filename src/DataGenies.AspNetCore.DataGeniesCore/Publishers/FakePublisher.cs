using System;
using System.Collections.Generic;

namespace DataGenies.AspNetCore.DataGeniesCore.Publishers
{
    public class FakePublisher : IPublisher
    {
        public void Publish(byte[] data)
        {
            
        }

        public void Publish(byte[] data, string routingKey)
        {
            throw new NotImplementedException();
        }

        public void Publish(byte[] data, IEnumerable<string> routingKeys)
        {
            throw new NotImplementedException();
        }

        public void PublishRange(IEnumerable<byte[]> dataRange)
        {
            
        }

        public void PublishRange(IEnumerable<byte[]> dataRange, string routingKey)
        {
            throw new NotImplementedException();
        }

        public void PublishTuples(IEnumerable<Tuple<byte[], string>> tuples)
        {
            throw new NotImplementedException();
        }
    }
}