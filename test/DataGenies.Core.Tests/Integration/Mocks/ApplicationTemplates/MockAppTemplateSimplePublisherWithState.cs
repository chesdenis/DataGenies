using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.Abstractions;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockAppTemplateSimplePublisherWithState : MockBasicAppTemplatePublisherWithState
    {
        private MockSampleProperties Properties => this.ContextContainer.Resolve<MockSampleProperties>();

        public MockAppTemplateSimplePublisherWithState(DataPublisherRole publisherRole) : base(publisherRole)
        {
            this.ContextContainer.Register<MockSampleProperties>(
                new MockSampleProperties
                {
                    PropertyA = "ABC",
                    PropertyB = "DEF"
                });
        }

        public override void Start()
        { 
            var testData = Encoding.UTF8.GetBytes("SamplePhrase");
            this.Publish(testData, Properties.PropertyA);
            this.Publish(testData, Properties.PropertyB);
        }

        public override void Stop()
        {
            
        }
    }
}