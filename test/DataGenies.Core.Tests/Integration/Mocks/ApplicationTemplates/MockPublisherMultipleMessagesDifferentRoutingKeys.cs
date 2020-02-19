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
    public class MockPublisherMultipleMessagesDifferentRoutingKeys : ManagedPublisherServiceWithContainer
    {
        public MockPublisherMultipleMessagesDifferentRoutingKeys(IContainer container, IPublisher publisher, IEnumerable<BehaviourTemplate> behaviourTemplates, IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours) : base(container, publisher, behaviourTemplates, wrapperBehaviours)
        {
            this.Container.Register<MockPublisherProperties>(new MockPublisherProperties());
        }

        private MockPublisherProperties Properties => this.Container.Resolve<MockPublisherProperties>();
        
        protected override void OnStart()
        {
            for (int i = 0; i < 10; i++)
            {
                var testString = $"TestString-{i}";
                
                var testData = Encoding.UTF8.GetBytes(testString);
                this.Publish(new MqMessage()
                {
                    Body = testData,
                    RoutingKey = i.ToString()  
                });
              
                Properties.PublishedMessages.Add(testString);
            }
        }
 
        protected override void OnStop()
        {
            
        }
        
      
    }
}