using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Containers;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockPublisherMultipleMessagesDifferentRoutingKeys :   ApplicationPublisherRole, IApplicationWithContext
    {
        public IContainer ContextContainer { get; set; } = new Container();
        
        private MockPublisherProperties Properties => this.ContextContainer.Resolve<MockPublisherProperties>();
        
        public MockPublisherMultipleMessagesDifferentRoutingKeys(DataPublisherRole publisherRole) : base(publisherRole)
        {
            this.ContextContainer.Register<MockPublisherProperties>(new MockPublisherProperties());
        }
    
        public override void Start()
        {
            for (int i = 0; i < 10; i++)
            {
                var testString = $"TestString-{i}";
                
                var testData = Encoding.UTF8.GetBytes(testString);
                this.Publish(testData, i.ToString());
              
                Properties.PublishedMessages.Add(testString);
            }
        }
    
        public override void Stop()
        {
                
        }
    }
}