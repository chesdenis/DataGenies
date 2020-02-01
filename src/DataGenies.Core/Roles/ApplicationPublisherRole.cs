using System;
using System.Collections.Generic;
using DataGenies.Core.Publishers;

namespace DataGenies.Core.Roles
{
    public abstract class ApplicationPublisherRole : IPublisher, IStartable
    {
        private readonly DataPublisherRole _publisherRole;

        public ApplicationPublisherRole(DataPublisherRole publisherRole)
        {
            _publisherRole = publisherRole;
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
    }
}