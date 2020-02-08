using System;
using DataGenies.Core.Attributes;
using DataGenies.Core.Containers;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockBrokenReceiver : ApplicationReceiverRole, IApplicationWithContext
    {
        public IContainer ContextContainer { get; set; } = new Container();
        
        private MockReceiverProperties Properties => this.ContextContainer.Resolve<MockReceiverProperties>();
        
        public MockBrokenReceiver(DataReceiverRole receiverRole) : base(receiverRole)
        {
            this.ContextContainer.Register<MockReceiverProperties>(new MockReceiverProperties());
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