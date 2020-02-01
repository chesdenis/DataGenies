using System;
using System.Collections.Generic;
using DataGenies.AspNetCore.DataGeniesCore.Publishers;

namespace DataGenies.AspNetCore.DataGeniesCore.ApplicationTemplates
{
    public abstract class ApplicationPublisherTemplate : IPublisher, IStartable
    {
        private readonly BasicDataPublisher _publisher;

        public ApplicationPublisherTemplate(BasicDataPublisher publisher)
        {
            _publisher = publisher;
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