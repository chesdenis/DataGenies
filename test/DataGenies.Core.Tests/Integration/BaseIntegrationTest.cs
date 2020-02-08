using DataGenies.Core.Roles;
using DataGenies.Core.Scanners;
using DataGenies.InMemory;
using NSubstitute;

namespace DataGenies.Core.Tests.Integration
{
    public class BaseIntegrationTest
    {
        protected InMemoryMqBroker InMemoryInMemoryMqBroker;

        protected InMemoryMqConfigurator InMemoryMqConfigurator;
        
        protected InMemorySchemaDataContext InMemoryInMemorySchemaContext;
        
        protected InMemorySchemaDataBuilder InMemorySchemaDataBuilder;
        
        protected ManagedApplicationBuilder _managedApplicationBuilder;
        
        protected InMemoryOrchestrator _orchestrator;
        
        protected IApplicationTemplatesScanner _applicationTemplatesScanner;
        
        protected IApplicationBehavioursScanner _applicationBehavioursScanner;

        public virtual void Initialize()
        {
            InMemoryInMemoryMqBroker = new InMemoryMqBroker();
            
            var receiverBuilder = new InMemoryReceiverBuilder(InMemoryInMemoryMqBroker);
            var publisherBuilder = new InMemoryPublisherBuilder(InMemoryInMemoryMqBroker);
            
            _applicationTemplatesScanner = Substitute.For<IApplicationTemplatesScanner>();
            _applicationBehavioursScanner = Substitute.For<IApplicationBehavioursScanner>();
            
            InMemoryInMemorySchemaContext = new InMemorySchemaDataContext();
            InMemorySchemaDataBuilder = new InMemorySchemaDataBuilder(InMemoryInMemorySchemaContext);
            InMemoryMqConfigurator = new InMemoryMqConfigurator(InMemoryInMemoryMqBroker);
            
            _orchestrator = new InMemoryOrchestrator(InMemoryInMemorySchemaContext,
                _applicationTemplatesScanner,
                _applicationBehavioursScanner,
                new ManagedApplicationBuilder(InMemoryInMemorySchemaContext, new InMemoryReceiverBuilder(InMemoryInMemoryMqBroker),
                    new InMemoryPublisherBuilder(InMemoryInMemoryMqBroker), InMemoryMqConfigurator));
        }
    }
}