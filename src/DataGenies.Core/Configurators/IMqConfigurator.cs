namespace DataGenies.Core.Configurators
{
    public interface IMqConfigurator
    {
        void EnsureExchange(string exchangeName);

        void EnsureQueue(string queueName, string exchangeName, string routingKey);
    }
}