using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Configurators;
using DataGenies.Core.Models;
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
        
        protected IBehaviourTemplatesScanner BehaviourTemplatesScanner;
        
        public virtual void Initialize()
        {
            InMemoryMqBroker = new InMemoryMqBroker();
             
            ApplicationTemplatesScanner = Substitute.For<IApplicationTemplatesScanner>();
            BehaviourTemplatesScanner = Substitute.For<IBehaviourTemplatesScanner>();
           
            InMemorySchemaContext = new InMemorySchemaDataContext();
            InMemorySchemaDataBuilder = new InMemorySchemaDataBuilder(InMemorySchemaContext);
            InMemoryMqConfigurator = new InMemoryMqConfigurator(InMemoryMqBroker);
            
            BindingConfigurator = new BindingConfigurator(InMemorySchemaContext, InMemoryMqConfigurator);
            
            Orchestrator = new InMemoryOrchestrator(InMemorySchemaContext,
                ApplicationTemplatesScanner,
                BehaviourTemplatesScanner,
               
                new ManagedServiceBuilder(
                    new InMemoryReceiverBuilder(InMemoryMqBroker),
                    new InMemoryPublisherBuilder(InMemoryMqBroker),
                    BindingConfigurator
                    ));
        }
    }
}