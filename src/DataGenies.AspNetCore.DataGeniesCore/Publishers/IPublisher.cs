using System.Collections.Generic;

namespace DataGenies.AspNetCore.DataGeniesCore.Publishers
{
    public interface IPublisher
    {
        void Publish(byte[] data);
        
        void PublishRange(IEnumerable<byte[]> dataRange);
    }
}