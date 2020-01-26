using System.Collections.Generic;

namespace DataGenies.AspNetCore.DataGeniesCore.Abstractions.Publishers
{
    public interface IPublisher
    {
        void Publish(byte[] data);
        
        void PublishRange(IEnumerable<byte[]> dataRange);
    }
}