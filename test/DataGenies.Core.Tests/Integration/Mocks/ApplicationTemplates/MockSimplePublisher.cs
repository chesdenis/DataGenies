using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Containers;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockSimplePublisher :  ApplicationPublisherRole, IApplicationWithContext
    {
        public IContainer ContextContainer { get; set; } = new Container();
        
        private MockPublisherProperties Properties => this.ContextContainer.Resolve<MockPublisherProperties>();
        
        public MockSimplePublisher(DataPublisherRole publisherRole) : base(publisherRole)
        {
            this.ContextContainer.Register<MockPublisherProperties>(new MockPublisherProperties());
        }

        public override void Start()
        {
            var testString = $"{this.Properties.ManagedParameter}TestString";
            
            var testData = Encoding.UTF8.GetBytes(testString);
            this.Publish(testData);
            
            Properties.PublishedMessages.Add(testString);
        }

        public override void Stop()
        {
                
        }
    }
}