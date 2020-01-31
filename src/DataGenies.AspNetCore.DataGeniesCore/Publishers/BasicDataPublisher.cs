using System;
using System.Collections.Generic;
using System.Linq;
using DataGenies.AspNetCore.DataGeniesCore.Converters;

namespace DataGenies.AspNetCore.DataGeniesCore.Publishers
{
    public class BasicDataPublisher : IPublisher
    {
        private readonly IPublisher publisher;
        private readonly IEnumerable<IConverter> converters;
         
        private IConverter[] BeforePublish() => this.converters.Where(w => w.Type == ConverterType.BeforePublish).ToArray();

        public BasicDataPublisher(IPublisher publisher, IEnumerable<IConverter> converters)
        {
            this.publisher = publisher;
            this.converters = converters;
        }

        public void Publish(byte[] data)
        {
            foreach (var converter in BeforePublish())
            {
                data = converter.Convert(data);
            }

            this.publisher.Publish(data);
        }

        public void Publish(byte[] data, string routingKey)
        {
            publisher.Publish(data, routingKey);
        }

        public void Publish(byte[] data, IEnumerable<string> routingKeys)
        {
            publisher.Publish(data, routingKeys);
        }

        public void PublishRange(IEnumerable<byte[]> dataRange)
        {
            var dataRangeToPublish = new List<byte[]>();

            foreach (var data in dataRange)
            {
                var dataToPublish = data;
                
                foreach (var converter in BeforePublish())
                {
                    dataToPublish = converter.Convert(data);
                }

                dataRangeToPublish.Add(dataToPublish);
            }
             
            this.publisher.PublishRange(dataRangeToPublish);
        }

        public void PublishRange(IEnumerable<byte[]> dataRange, string routingKey)
        {
            publisher.PublishRange(dataRange, routingKey);
        }

        public void PublishTuples(IEnumerable<Tuple<byte[], string>> tuples)
        {
            publisher.PublishTuples(tuples);
        }
    }
}