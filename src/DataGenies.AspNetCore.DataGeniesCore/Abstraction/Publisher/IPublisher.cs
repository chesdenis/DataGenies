using System.Collections.Generic;

namespace DataGenies.AspNetCore.DataGeniesCore.Abstraction.Publisher
{
    public interface IPublisher
    {
        void Publish(byte[] data);
        
        void PublishRange(IEnumerable<byte[]> dataRange);
    }
}