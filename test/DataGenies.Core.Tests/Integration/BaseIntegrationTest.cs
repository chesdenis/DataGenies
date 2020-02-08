using DataGenies.Core.Roles;
using DataGenies.Core.Scanners;
using DataGenies.InMemory;
using NSubstitute;

namespace DataGenies.Core.Tests.Integration
{
    public class BaseIntegrationTest
    {
        protected InMemoryMqBroker InMemoryMqBroker;

        protected InMemoryMqConfigurator InMemoryMqConfigurator;
        
        protected InMemorySchemaDataContext InMemorySchemaContext;
        
        protected InMemorySchemaDataBuilder InMemorySchemaDataBuilder;

        protected InMemoryOrchestrator Orchestrator;
        
        protected IApplicationTemplatesScanner ApplicationTemplatesScanner;
        
        protected IApplicationBehavioursScanner ApplicationBehavioursScanner;
        protected IApplicationConvertersScanner ApplicationConvertersScanner;

        public virtual void Initialize()
        {
            InMemoryMqBroker = new InMemoryMqBroker();
             
            ApplicationTemplatesScanner = Substitute.For<IApplicationTemplatesScanner>();
            ApplicationBehavioursScanner = Substitute.For<IApplicationBehavioursScanner>();
            ApplicationConvertersScanner = Substitute.For<IApplicationConvertersScanner>();
            
            InMemorySchemaContext = new InMemorySchemaDataContext();
            InMemorySchemaDataBuilder = new InMemorySchemaDataBuilder(InMemorySchemaContext);
            InMemoryMqConfigurator = new InMemoryMqConfigurator(InMemoryMqBroker);
            
            Orchestrator = new InMemoryOrchestrator(InMemorySchemaContext,
                ApplicationTemplatesScanner,
                ApplicationBehavioursScanner,
                ApplicationConvertersScanner,
                new ManagedApplicationBuilder(InMemorySchemaContext, new InMemoryReceiverBuilder(InMemoryMqBroker),
                    new InMemoryPublisherBuilder(InMemoryMqBroker), InMemoryMqConfigurator));
        }
    }
}