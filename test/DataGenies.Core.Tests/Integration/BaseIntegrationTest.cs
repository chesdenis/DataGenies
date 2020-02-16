using DataGenies.Core.Configurators;
using DataGenies.Core.Scanners;
using DataGenies.Core.Services;
using DataGenies.InMemory;
using NSubstitute;

namespace DataGenies.Core.Tests.Integration
{
    public class BaseIntegrationTest
    {
        protected InMemoryMqBroker InMemoryMqBroker;

        protected InMemoryMqConfigurator InMemoryMqConfigurator;

        protected IBindingConfigurator BindingConfigurator;
        
        protected InMemorySchemaDataContext InMemorySchemaContext;
        
        protected InMemorySchemaDataBuilder InMemorySchemaDataBuilder;

        protected InMemoryOrchestrator Orchestrator;
        
        protected IApplicationTemplatesScanner ApplicationTemplatesScanner;
        
        protected IApplicationBehavioursScanner ApplicationBehavioursScanner;
        
        public virtual void Initialize()
        {
            InMemoryMqBroker = new InMemoryMqBroker();
             
            ApplicationTemplatesScanner = Substitute.For<IApplicationTemplatesScanner>();
            ApplicationBehavioursScanner = Substitute.For<IApplicationBehavioursScanner>();
           
            InMemorySchemaContext = new InMemorySchemaDataContext();
            InMemorySchemaDataBuilder = new InMemorySchemaDataBuilder(InMemorySchemaContext);
            InMemoryMqConfigurator = new InMemoryMqConfigurator(InMemoryMqBroker);
            
            BindingConfigurator = new BindingConfigurator(InMemorySchemaContext, InMemoryMqConfigurator);
            
            Orchestrator = new InMemoryOrchestrator(InMemorySchemaContext,
                ApplicationTemplatesScanner,
                ApplicationBehavioursScanner,
               
                new ManagedServiceBuilder(
                    new InMemoryReceiverBuilder(InMemoryMqBroker),
                    new InMemoryPublisherBuilder(InMemoryMqBroker),
                    BindingConfigurator
                    ));
        }
    }
}