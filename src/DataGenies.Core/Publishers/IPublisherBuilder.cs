namespace DataGenies.Core.Publishers
{
    public interface IPublisherBuilder
    {
        IPublisherBuilder WithExchange(string exchangeName);
            
        public IPublisher Build();
    }
}