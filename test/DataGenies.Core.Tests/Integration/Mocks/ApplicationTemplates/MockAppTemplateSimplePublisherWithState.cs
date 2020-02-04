using System.Runtime.CompilerServices;
using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.Abstractions;
using DataGenies.Core.Tests.Integration.Mocks.Behaviours;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    [BehaviourTemplate(typeof(MockSimpleBeforeStartBehaviour))]
    public class MockAppTemplateSimplePublisherWithState : MockBasicAppTemplatePublisherWithState
    {
        public MockAppTemplateSimplePublisherWithState(DataPublisherRole publisherRole) : base(publisherRole)
        {
            this.Properties = new MockSampleProperties
            {
                PropertyA = "ABC",
                PropertyB = "DEF"
            };
        }

        public override void Start()
        {
            var properties = (MockSampleProperties) this.Properties;

            var testData = Encoding.UTF8.GetBytes("SamplePhrase");
            this.Publish(new byte[] {1, 2, 3, 4}, properties.PropertyA);
            this.Publish(new byte[] {1, 2, 3, 4}, properties.PropertyB);
        }

        public override void Stop()
        {
            
        }
    }
}