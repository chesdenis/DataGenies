using System.Collections.Generic;
using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Services;
using DataGenies.Core.Tests.Integration.Mocks.Properties;
using DataGenies.Core.Wrappers;
using DataGenies.InMemory;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockSimplePublisher :  ManagedCommunicableServiceWithContainer
    {
        public MockSimplePublisher(IContainer container, IPublisher publisher, IReceiver receiver,
            IEnumerable<BehaviourTemplate> behaviourTemplates, IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours)
            : base(container, publisher, receiver, behaviourTemplates, wrapperBehaviours)
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