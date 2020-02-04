﻿using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.Abstractions;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockAppTemplatePublisherWhichPushMultipleMessagesWithDifferentRoutingKeys : MockBasicAppTemplatePublisher
    {
        public MockAppTemplatePublisherWhichPushMultipleMessagesWithDifferentRoutingKeys(DataPublisherRole publisherRole) : base(publisherRole)
        {
        }

        public override void Start()
        {
            for (int i = 0; i < 10; i++)
            {
                var testData = Encoding.UTF8.GetBytes($"TestString-{i}");
                this.Publish(testData, i.ToString());
                State.Add(testData);
            }
        }

        public override void Stop()
        {
                
        }
    }
}