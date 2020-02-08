using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Containers;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockSimpleReceiverAndPublisher : ApplicationReceiverAndPublisherRole, IApplicationWithContext
    {
        public IContainer ContextContainer { get; set; } = new Container();
        
        private MockPublisherProperties PublisherProperties => this.ContextContainer.Resolve<MockPublisherProperties>();
        
        private MockReceiverProperties ReceiverProperties => this.ContextContainer.Resolve<MockReceiverProperties>();

        public MockSimpleReceiverAndPublisher(DataReceiverRole receiverRole, DataPublisherRole publisherRole) : base(
            receiverRole, publisherRole)
        {
            this.ContextContainer.Register<MockPublisherProperties>(new MockPublisherProperties());
            this.ContextContainer.Register<MockReceiverProperties>(new MockReceiverProperties());
        }

        public override void Start()
        {
            this.Listen((message) =>
            {
                var testData = Encoding.UTF8.GetString(message);
                ReceiverProperties.ReceivedMessages.Add(testData);
                
                var changedTestData = $"{testData}-changed!";
    
                var bytes = Encoding.UTF8.GetBytes(changedTestData);
                this.Publish( bytes);
                
                PublisherProperties.PublishedMessages.Add(changedTestData);
            });
        }
    
        public override void Stop()
        {
            this.StopListen();
        }
    }
}