using System;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.Behaviours
{
    [BehaviourTemplate]
    public class MockBehaviour : BehaviourTemplate
    {  
        public override void Execute(IContainer arg)
        {
            throw new NotImplementedException();
        }
    }
}