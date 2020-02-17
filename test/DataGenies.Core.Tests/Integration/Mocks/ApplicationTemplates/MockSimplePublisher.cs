using System.Collections.Generic;
using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Publishers;
using DataGenies.Core.Services;
using DataGenies.Core.Tests.Integration.Mocks.Properties;
using DataGenies.Core.Wrappers;
using DataGenies.InMemory;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockSimplePublisher :  ManagedPublisherServiceWithContainer
    {
        public MockSimplePublisher(
            IContainer container,
            IPublisher publisher,
            IEnumerable<IBasicBehaviour> basicBehaviours, 
            IEnumerable<IBehaviourOnException> behaviourOnExceptions,
            IEnumerable<IWrapperBehaviour> wrapperBehaviours) 
            : base(
                container, 
                publisher, 
                basicBehaviours, 
                behaviourOnExceptions, 
                wrapperBehaviours)
        {
            this.Container.Register<MockPublisherProperties>(new MockPublisherProperties());
        }
 
        protected MockPublisherProperties Properties => this.Container.Resolve<MockPublisherProperties>();
        
        protected override void OnStart()
        {
            throw new System.NotImplementedException();
        }
 
        protected override void OnStop()
        {
            throw new System.NotImplementedException();
        }
    }
}