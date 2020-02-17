using System.Collections.Generic;
using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Services;
using DataGenies.Core.Tests.Integration.Mocks.Properties;
using DataGenies.Core.Wrappers;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockSimpleReceiverAndPublisher : ManagedReceiverAndPublisherServiceWithContainer
    {
        public MockSimpleReceiverAndPublisher(IContainer container, IPublisher publisher, IReceiver receiver,
            IEnumerable<IBasicBehaviour> basicBehaviours, IEnumerable<IBehaviourOnException> behaviourOnExceptions,
            IEnumerable<IWrapperBehaviour> wrapperBehaviours) : base(container, publisher, receiver, basicBehaviours,
            behaviourOnExceptions, wrapperBehaviours)
        {
            this.Container.Register<MockPublisherProperties>(new MockPublisherProperties());
            this.Container.Register<MockReceiverProperties>(new MockReceiverProperties());
        }
 
        private MockPublisherProperties PublisherProperties => this.Container.Resolve<MockPublisherProperties>();
        
        private MockReceiverProperties ReceiverProperties => this.Container.Resolve<MockReceiverProperties>();
        
        protected override void OnStart()
        {
            this.Listen((message) =>
            {
                var testData = Encoding.UTF8.GetString(message.Body);
                ReceiverProperties.ReceivedMessages.Add(testData);
                
                var changedTestData = $"{testData}-changed!";
    
                var bytes = Encoding.UTF8.GetBytes(changedTestData);
                message.Body = bytes;
                this.Publish(message);
                
                PublisherProperties.PublishedMessages.Add(changedTestData);
            });
        }
 
        protected override void OnStop()
        {
            this.StopListen();
        }
    }
}