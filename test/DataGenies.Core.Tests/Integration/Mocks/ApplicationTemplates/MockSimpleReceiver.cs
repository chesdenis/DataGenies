using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Containers;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockSimpleReceiver : ApplicationReceiverRole, IApplicationWithContext
    {
        public IContainer ContextContainer { get; set; } = new Container();
        
        private MockReceiverProperties Properties => this.ContextContainer.Resolve<MockReceiverProperties>();

        
        public MockSimpleReceiver(DataReceiverRole receiverRole) : base(receiverRole)
        {
            this.ContextContainer.Register<MockReceiverProperties>(new MockReceiverProperties());
        }
    
        public override void Start()
        {
            this.Listen((message) =>
            {
                Properties.ReceivedMessages.Add(
                    Encoding.UTF8.GetString(message));
            });
        }
    
        public override void Stop()
        {
            this.StopListen();
        }
    }
}