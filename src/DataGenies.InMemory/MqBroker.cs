using System;
using System.Collections.Generic;

namespace DataGenies.InMemory
{
    public class MqBroker
    {
        public IEnumerable<Tuple<string, Queue>> Model { get; set; }

        public MqBroker()
        {
            this.Model = new List<Tuple<string, Queue>>();
        }
    }
}