using System.Collections.Generic;
using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Converters;
using DataGenies.Core.Receivers;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockSimpleReceiver : ApplicationReceiverRole, IApplicationWithContext
    {
        public MockSimpleReceiver(IReceiver receiver, IEnumerable<IBehaviour> behaviours, IEnumerable<IConverter> converters) : base(receiver, behaviours, converters)
        {
            this.ContextContainer.Register<MockReceiverProperties>(new MockReceiverProperties());
        }

        public IContainer ContextContainer { get; set; } = new Container();
        
        private MockReceiverProperties Properties => this.ContextContainer.Resolve<MockReceiverProperties>();

        
        
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