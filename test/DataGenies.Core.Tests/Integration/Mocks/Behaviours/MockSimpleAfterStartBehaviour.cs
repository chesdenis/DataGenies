using System;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.Behaviours
{
    [BehaviourTemplate]
    public class MockSimpleAfterStartBehaviour : GenericBehaviour<MockBehaviourProperties>
    { 
        public override BehaviourType Type { get; set; } = BehaviourType.AfterStart;
        
        public override void DoSomethingBeforeStart()
        {
            throw new NotImplementedException();
        }
    
        public override void DoSomethingAfterStart()
        {
            this.ContextContainer.Resolve<MockPublisherProperties>().ManagedParameter = "Prefix";
        }
    
        public override void DoSomethingDuringRunning(Action action)
        {
            throw new NotImplementedException();
        }
    
        public override void DoSomethingOnException(Exception ex = null)
        {
            throw new NotImplementedException();
        }
    }
}