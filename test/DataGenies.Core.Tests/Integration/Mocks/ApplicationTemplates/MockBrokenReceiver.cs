using System;
using System.Collections.Generic;
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
    public class MockBrokenReceiver : ApplicationReceiverRole, IApplicationWithContext
    {
        public MockBrokenReceiver(IReceiver receiver, IEnumerable<IBehaviour> behaviours, IEnumerable<IConverter> converters) : base(receiver, behaviours, converters)
        {
            this.ContextContainer.Register<MockReceiverProperties>(new MockReceiverProperties());
        }

        public IContainer ContextContainer { get; set; } = new Container();
        
        private MockReceiverProperties Properties => this.ContextContainer.Resolve<MockReceiverProperties>();
        
        
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