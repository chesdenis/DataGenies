using System.Collections.Generic;
using DataGenies.Core.Models;

namespace DataGenies.Core.Services
{
    public interface IPublisherManagedService
    {
        void Publish(MqMessage data);

        void PublishScoped(IEnumerable<VirtualBinding> scope, MqMessage data);
        
        void PublishRange(IEnumerable<MqMessage> dataRange);
        
        void PublishRangeScoped(IEnumerable<VirtualBinding> scope, IEnumerable<MqMessage> dataRange);
    }
}