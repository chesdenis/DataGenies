using System.Collections.Generic;

namespace DataGenies.AspNetCore.DataGeniesCore.Publishers
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