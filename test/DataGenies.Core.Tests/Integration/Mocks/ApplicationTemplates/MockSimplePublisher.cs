using System.Collections.Generic;
using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Publishers;
using DataGenies.Core.Services;
using DataGenies.Core.Tests.Integration.Mocks.Properties;
using DataGenies.Core.Wrappers;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockSimplePublisher :  ManagedPublisherServiceWithContainer
    {
        public MockSimplePublisher(IContainer container, IPublisher publisher,
            IEnumerable<IBasicBehaviour> basicBehaviours, IEnumerable<IBehaviourOnException> behaviourOnExceptions,
            IEnumerable<IWrapperBehaviour> wrapperBehaviours) : base(container, publisher, basicBehaviours,
            behaviourOnExceptions, wrapperBehaviours)
        {
            this.Container.Register<MockPublisherProperties>(new MockPublisherProperties());
        }
 
        private MockPublisherProperties Properties => this.Container.Resolve<MockPublisherProperties>();
        
        protected override void OnStart()
        {
            var testString = $"{this.Properties.ManagedParameter}TestString";
            
            var testData = Encoding.UTF8.GetBytes(testString);
            this.Publish(testData);
            
            Properties.PublishedMessages.Add(testString);
        }
 
        protected override void OnStop()
        {
            throw new System.NotImplementedException();
        }
    }
}