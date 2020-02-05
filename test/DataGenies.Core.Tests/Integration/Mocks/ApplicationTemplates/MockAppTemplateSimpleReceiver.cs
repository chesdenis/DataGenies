﻿using DataGenies.Core.Attributes;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.Abstractions;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
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