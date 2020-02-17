using System;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.Behaviours
{
    [BehaviourTemplate]
    public class MockBehaviour : BasicBehaviour
    {  
        public override void Execute(IContainer arg)
        {
            throw new NotImplementedException();
        }

        public override BehaviourScope BehaviourScope { get; set; } = BehaviourScope.Service;
        public override BehaviourType BehaviourType { get; set; } = BehaviourType.AfterStart;
    }
}