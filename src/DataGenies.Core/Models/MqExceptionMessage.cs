using System;

namespace DataGenies.Core.Models
{
    public class MqExceptionMessage : MqMessage
    {
        public Exception Exception { get; set; }
    }
}