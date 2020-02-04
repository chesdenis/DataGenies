using System;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.Behaviours
{
    public class MockSimpleBeforeStartBehaviour : GenericBehaviour
    {
        public override BehaviourType Type { get; set; } = BehaviourType.BeforeStart;
        
        public List<string> SomeData = new List<string>();
        
        public override void DoSomethingBeforeStart()
        {
            var properties = (MockSampleProperties) GetApplicationProperties();
            
            SomeData.Add(properties.PropertyA);
            SomeData.Add(properties.PropertyB);
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