namespace DG.Core.InMemoryMessageBroker
{
    public interface IMqConfigurator
    {
        void EnsureExchange(string exchangeName);

        void EnsureQueue(string queueName, string exchangeName, string routingKey);
    }
}
