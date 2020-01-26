using System.Collections.Generic;

namespace DataGenies.AspNetCore.DataGeniesCore.Abstraction.Publisher
{
    public class FakePublisher : IPublisher
    {
        public void Publish(byte[] data)
        {
            
        }

        public void PublishRange(IEnumerable<byte[]> dataRange)
        {
            
        }
    }
}