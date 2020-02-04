using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.Abstractions;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockAppTemplateSimplePublisher : MockBasicAppTemplatePublisher
    {
        public MockAppTemplateSimplePublisher(DataPublisherRole publisherRole) : base(publisherRole)
        {
        }

        public override void Start()
        {
            var testData = Encoding.UTF8.GetBytes("TestString");
            this.Publish(testData);
            State.Add(testData);
        }

        public override void Stop()
        {
                
        }
    }
}