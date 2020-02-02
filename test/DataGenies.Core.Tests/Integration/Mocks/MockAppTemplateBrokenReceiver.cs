using System;
using DataGenies.Core.Attributes;
using DataGenies.Core.Roles;

namespace DataGenies.Core.Tests.Integration.Mocks
{
    [ApplicationTemplate]
    public class MockAppTemplateBrokenReceiver : MockBasicAppTemplateReceiver
    {
        public MockAppTemplateBrokenReceiver(DataReceiverRole receiverRole) : base(receiverRole)
        {
        }

        public override void Start()
        {
            this.Listen((message) =>
            {
                throw new Exception("Something went wrong");
            });
        }

        public override void Stop()
        {
            this.StopListen();
        }
    }
}