using DataGenies.Core.Roles;
using DataGenies.Core.Scanners;
using DataGenies.InMemory;
using NSubstitute;

namespace DataGenies.Core.Tests.Integration
{
    public class BaseIntegrationTest
    {
        protected MqBroker _inMemoryMqBroker;

        protected MqConfigurator _mqConfigurator;
        
        protected SchemaDataContext _inMemorySchemaContext;
        
        protected SchemaDataBuilder _schemaDataBuilder;
        
        protected ManagedApplicationBuilder _managedApplicationBuilder;
        
        protected InMemoryOrchestrator _orchestrator;
        
        protected IApplicationTemplatesScanner _applicationTemplatesScanner;
        
        protected IApplicationBehavioursScanner _applicationBehavioursScanner;

        public virtual void Initialize()
        {
            _inMemoryMqBroker = new MqBroker();
            
            var receiverBuilder = new ReceiverBuilder(_inMemoryMqBroker);
            var publisherBuilder = new PublisherBuilder(_inMemoryMqBroker);
            
            _applicationTemplatesScanner = Substitute.For<IApplicationTemplatesScanner>();
            _applicationBehavioursScanner = Substitute.For<IApplicationBehavioursScanner>();
            
            _inMemorySchemaContext = new SchemaDataContext();
            _schemaDataBuilder = new SchemaDataBuilder(_inMemorySchemaContext);
            _mqConfigurator = new MqConfigurator(_inMemoryMqBroker);
            
            _orchestrator = new InMemoryOrchestrator(_inMemorySchemaContext,
                _applicationTemplatesScanner,
                _applicationBehavioursScanner,
                new ManagedApplicationBuilder(_inMemorySchemaContext, new ReceiverBuilder(_inMemoryMqBroker),
                    new PublisherBuilder(_inMemoryMqBroker), _mqConfigurator));
        }
    }
}