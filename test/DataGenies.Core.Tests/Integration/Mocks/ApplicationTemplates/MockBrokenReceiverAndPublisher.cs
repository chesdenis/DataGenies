using System;
using DataGenies.Core.Attributes;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockBrokenReceiverAndPublisher : ApplicationReceiverAndPublisherRole, IApplicationWithContext
    {
        public MockBrokenReceiverAndPublisher(ApplicationReceiverRole receiverRole, ApplicationPublisherRole publisherRole) : base(receiverRole, publisherRole)
        {
            this.ContextContainer.Register<ApplicationReceiverAndPublisherRole>(this);
            this.ContextContainer.Register<MockReceiverProperties>(new MockReceiverProperties());
        }

        public IContainer ContextContainer { get; } = new Container();
        private MockReceiverProperties ReceiverProperties => this.ContextContainer.Resolve<MockReceiverProperties>();
        
        public override void Start()
        {
            this.Listen((message) =>
            {
                ReceiverProperties.ReceivedMessages.Add(message.FromBytes<string>());
                throw new Exception("Something went wrong");
            });
        }

        public override void Stop()
        {
            this.StopListen();
        }
    }
}