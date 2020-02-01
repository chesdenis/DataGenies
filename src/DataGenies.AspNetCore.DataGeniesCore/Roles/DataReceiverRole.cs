using System;
using System.Collections.Generic;
using System.Linq;
using DataGenies.AspNetCore.DataGeniesCore.Converters;
using DataGenies.AspNetCore.DataGeniesCore.Receivers;

namespace DataGenies.AspNetCore.DataGeniesCore.Roles
{
    public class DataReceiverRole : IReceiver
    {
        private readonly IReceiver receiver;
        private readonly IEnumerable<IConverter> converters;

        private IConverter[] AfterReceive() => this.converters.Where(w => w.Type == ConverterType.AfterReceive).ToArray();
        
        public DataReceiverRole(IReceiver receiver, IEnumerable<IConverter> converters)
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

        public void StopListen() => this.receiver.StopListen();
    }
}