using System.Collections.Generic;
using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Configurators;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;
using DataGenies.Core.Models;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Services;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockSimplePublisher :  ManagedService
    {
        public MockSimplePublisher(IContainer container, IPublisher publisher, IReceiver receiver, IEnumerable<BehaviourTemplate> behaviourTemplates, IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours, ISchemaDataContext schemaDataContext, IBindingConfigurator bindingConfigurator) : base(container, publisher, receiver, behaviourTemplates, wrapperBehaviours, schemaDataContext, bindingConfigurator)
        {
            this.Container.Register<MockPublisherProperties>(new MockPublisherProperties());
        }

        protected MockPublisherProperties Properties => this.Container.Resolve<MockPublisherProperties>();
        
        protected override void OnStart()
        {
            this.Publish(new MqMessage(){ Body = "TestMessage".ToBytes(), RoutingKey = "#"});
        }
 
        protected override void OnStop()
        {
            
        }
    }
}