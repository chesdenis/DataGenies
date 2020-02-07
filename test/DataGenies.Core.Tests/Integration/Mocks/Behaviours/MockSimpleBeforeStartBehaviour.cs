using System;
using System.Collections.Generic;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.Behaviours
{
    [BehaviourTemplate]
    public class MockSimpleBeforeStartBehaviour : GenericBehaviour<MockSampleProperties>
    { 
        public override BehaviourType Type { get; set; } = BehaviourType.BeforeStart;
        
        public List<string> SomeData = new List<string>();
        
        public override void DoSomethingBeforeStart()
        {
            SomeData.Add(this.StateContainer.Resolve<MockSampleProperties>().PropertyA);
            SomeData.Add(this.StateContainer.Resolve<MockSampleProperties>().PropertyB);
        }

        public override void DoSomethingAfterStart()
        {
            throw new NotImplementedException();
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