using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using DataGenies.AspNetCore.DataGeniesCore.Publishers;
using DataGenies.AspNetCore.DataGeniesCore.Receivers;

namespace DataGenies.AspNetCore.DataGeniesCore.Components
{
    public abstract class ApplicationReceiverPublisherType : IReceiver, IPublisher, IStartable
    {
        private readonly BasicDataReceiver _receiver;
        private readonly BasicDataPublisher _publisher;

        protected ApplicationReceiverPublisherType(BasicDataReceiver receiver, BasicDataPublisher publisher)
        {
            _receiver = receiver;
            _publisher = publisher;
        }

        public void Listen(Action<byte[]> onReceive)
        {
            _receiver.Listen(onReceive);
        }

        public void StopListen()
        {
            _receiver.StopListen();
        }

        public void Publish(byte[] data)
        {
            _publisher.Publish(data);
        }

        public void Publish(byte[] data, string routingKey)
        {
            _publisher.Publish(data, routingKey);
        }

        public void Publish(byte[] data, IEnumerable<string> routingKeys)
        {
            _publisher.Publish(data, routingKeys);
        }

        public void PublishRange(IEnumerable<byte[]> dataRange)
        {
            _publisher.PublishRange(dataRange);
        }

        public void PublishRange(IEnumerable<byte[]> dataRange, string routingKey)
        {
            _publisher.PublishRange(dataRange, routingKey);
        }

        public void PublishTuples(IEnumerable<Tuple<byte[], string>> tuples)
        {
            _publisher.PublishTuples(tuples);
        }

        public abstract void Start();
    }
}