using System;
using System.Collections.Generic;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;

namespace DataGenies.Core.Roles
{
    public abstract class ApplicationReceiverAndPublisherRole : IReceiver, IPublisher, IRestartable
    {
        private readonly DataReceiverRole _receiverRole;
        private readonly DataPublisherRole _publisherRole;

        protected ApplicationReceiverAndPublisherRole(DataReceiverRole receiverRole, DataPublisherRole publisherRole)
        {
            _receiverRole = receiverRole;
            _publisherRole = publisherRole;
        }

        public void Listen(Action<byte[]> onReceive)
        {
            _receiverRole.Listen(onReceive);
        }

        public void StopListen()
        {
            _receiverRole.StopListen();
        }

        public void Publish(byte[] data)
        {
            _publisherRole.Publish(data);
        }

        public void Publish(byte[] data, string routingKey)
        {
            _publisherRole.Publish(data, routingKey);
        }

        public void Publish(byte[] data, IEnumerable<string> routingKeys)
        {
            _publisherRole.Publish(data, routingKeys);
        }

        public void PublishRange(IEnumerable<byte[]> dataRange)
        {
            _publisherRole.PublishRange(dataRange);
        }

        public void PublishRange(IEnumerable<byte[]> dataRange, string routingKey)
        {
            _publisherRole.PublishRange(dataRange, routingKey);
        }

        public void PublishTuples(IEnumerable<Tuple<byte[], string>> tuples)
        {
            _publisherRole.PublishTuples(tuples);
        }

        public abstract void Start();
        public abstract void Stop();
    }
}