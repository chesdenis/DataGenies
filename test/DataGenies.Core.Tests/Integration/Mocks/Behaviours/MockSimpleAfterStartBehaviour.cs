using System;
using System.Security.Cryptography.X509Certificates;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Receivers;
using DataGenies.Core.Tests.Integration.Mocks.Properties;
using DataGenies.InMemory;
using Newtonsoft.Json;

namespace DataGenies.Core.Tests.Integration.Mocks.Behaviours
{
    public class MqExceptionMessage : MqMessage
    {
        public Exception Exception { get; set; }
    }


    [BehaviourTemplate]
    public class MockSimpleAfterStartBehaviour : GenericBehaviour<MockBehaviourProperties>
    { 
        public override BehaviourType Type { get; set; } = BehaviourType.AfterStart;
        
        public override void DoSomethingAfterStart()
        {
            this.ContextContainer.Resolve<MockPublisherProperties>().ManagedParameter = "Prefix";
        }
    }
}