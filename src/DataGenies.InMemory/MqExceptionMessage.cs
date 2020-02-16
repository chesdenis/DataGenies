using System;

namespace DataGenies.InMemory
{
    public class MqExceptionMessage : MqMessage
    {
        public Exception Exception { get; set; }
    }
}