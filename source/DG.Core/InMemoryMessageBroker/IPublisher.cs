namespace DG.Core.InMemoryMessageBroker
{
    using System.Collections.Generic;
    using DG.Core.Model.Message;

    public interface IPublisher
    {
        void Publish(string exchange, MqMessage data);

        void PublishRange(string exchange, IEnumerable<MqMessage> dataRange);
    }
}
