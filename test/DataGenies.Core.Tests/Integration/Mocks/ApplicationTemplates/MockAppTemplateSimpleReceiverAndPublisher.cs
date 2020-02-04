using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.Abstractions;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockAppTemplateSimpleReceiverAndPublisher : MockBasicAppTemplateReceiverAndPublisher
    {
        public MockAppTemplateSimpleReceiverAndPublisher(DataReceiverRole receiverRole, DataPublisherRole publisherRole) : base(receiverRole, publisherRole)
        {
        }

        public override void Start()
        {
            this.Listen((message) =>
            {
                var testData = Encoding.UTF8.GetString(message);
                var changedTestData = $"{testData}-changed!";

                var bytes = Encoding.UTF8.GetBytes(changedTestData);
                this.Publish( bytes);
                State.Add(bytes);
            });
        }

        public override void Stop()
        {
            this.StopListen();
        }
    }
}