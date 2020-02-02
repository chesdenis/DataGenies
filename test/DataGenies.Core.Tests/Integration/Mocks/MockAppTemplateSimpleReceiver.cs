using DataGenies.Core.Attributes;
using DataGenies.Core.Roles;

namespace DataGenies.Core.Tests.Integration.Mocks
{
    [ApplicationTemplate]
    public class MockAppTemplateSimpleReceiver : MockBasicAppTemplateReceiver
    {
        public MockAppTemplateSimpleReceiver(DataReceiverRole receiverRole) : base(receiverRole)
        {
        }

        public override void Start()
        {
            this.Listen((message) => { State.Add(message); });
        }

        public override void Stop()
        {
            this.StopListen();
        }
    }
}