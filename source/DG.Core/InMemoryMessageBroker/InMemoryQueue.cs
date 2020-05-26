namespace DG.Core.InMemoryMessageBroker
{
    using System.Collections.Concurrent;
    using DG.Core.Model.Message;

    public class InMemoryQueue : ConcurrentQueue<MqMessage>
    {
        public string Name { get; set; }
    }
}
