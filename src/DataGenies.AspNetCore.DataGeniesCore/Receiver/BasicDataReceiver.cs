using System;
using System.Collections.Generic;
using System.Linq;
using DataGenies.AspNetCore.DataGeniesCore.Abstraction;

namespace DataGenies.AspNetCore.DataGeniesCore.Receiver
{
    public class BasicDataReceiver : IReceiver
    {
        private readonly IReceiver receiver;
        private readonly IEnumerable<IConverter> converters;

        private IConverter[] AfterReceive() => this.converters.Where(w => w.Type == ConverterType.AfterReceive).ToArray();
        
        public BasicDataReceiver(IReceiver receiver, IEnumerable<IConverter> converters)
        {
            this.receiver = receiver;
            this.converters = converters;
        }
       
        public void Listen(Action<byte[]> onReceive)
        {
            this.receiver.Listen((data) => 
            {
                foreach (var converter in AfterReceive())
                {
                    data = converter.Convert(data);
                }

                onReceive(data);
            });
        }
    }
}