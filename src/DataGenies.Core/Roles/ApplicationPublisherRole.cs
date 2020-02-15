using System;
using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Converters;
using DataGenies.Core.Publishers;

namespace DataGenies.Core.Roles
{
    public class ApplicationPublisherRole : IPublisher, IRestartable
    {
        private readonly IPublisher _publisher;
        private readonly IEnumerable<IBehaviour> _behaviours;
        private readonly IEnumerable<IConverter> _converters;
        
        private IConverter[] BeforePublish() => this._converters.Where(w => w.Type == ConverterType.BeforePublish).ToArray();
 
        public ApplicationPublisherRole(IPublisher publisher, IEnumerable<IBehaviour> behaviours, IEnumerable<IConverter> converters)
        {
            _publisher = publisher;
            _behaviours = behaviours;
            _converters = converters;
        }

        public void Publish(byte[] data)
        {
            foreach (var converter in BeforePublish())
            {
                data = converter.Convert(data);
            }

            this._publisher.Publish(data);
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
             
            this._publisher.PublishRange(dataRangeToPublish);
        }

        public void PublishRange(IEnumerable<byte[]> dataRange, string routingKey)
        {
            _publisher.PublishRange(dataRange, routingKey);
        }

        public void PublishTuples(IEnumerable<Tuple<byte[], string>> tuples)
        {
            _publisher.PublishTuples(tuples);
        }

        public virtual void Start() => throw new NotImplementedException();
        public virtual void Stop() => throw new NotImplementedException();
    }
}