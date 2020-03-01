using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Configurators;
using DataGenies.Core.InMemory;
using DataGenies.Core.InMemory.Messaging;
using DataGenies.Core.Models;
using DataGenies.Core.Scanners;
using DataGenies.Core.Services;
using DataGenies.Core.Tests.Integration.Stubs;
using DataGenies.Core.Tests.Integration.Stubs.Data;
using NSubstitute;

namespace DataGenies.Core.Tests.Integration
{
    public class BaseIntegrationTest
    {
        protected InMemoryMqBroker InMemoryMqBroker;

        protected InMemoryMqConfigurator InMemoryMqConfigurator;

        protected IBindingConfigurator BindingConfigurator;
        
        protected InMemoryFlowSchemaContext InMemoryFlowSchemaContext;
        
        protected InMemorySchemaDataBuilder InMemorySchemaDataBuilder;

        protected InMemoryOrchestrator Orchestrator;
        
        protected IApplicationTemplatesScanner ApplicationTemplatesScanner;
        
        protected IBehaviourTemplatesScanner BehaviourTemplatesScanner;
        
        public virtual void Initialize()
        {
            InMemoryMqBroker = new InMemoryMqBroker();
             
            ApplicationTemplatesScanner = Substitute.For<IApplicationTemplatesScanner>();
            BehaviourTemplatesScanner = Substitute.For<IBehaviourTemplatesScanner>();
           
            InMemoryFlowSchemaContext = new InMemoryFlowSchemaContext();
            InMemorySchemaDataBuilder = new InMemorySchemaDataBuilder(InMemoryFlowSchemaContext);
            InMemoryMqConfigurator = new InMemoryMqConfigurator(InMemoryMqBroker);
            
            BindingConfigurator = new BindingConfigurator(InMemoryFlowSchemaContext, InMemoryMqConfigurator);
            
            Orchestrator = new InMemoryOrchestrator(InMemoryFlowSchemaContext,
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