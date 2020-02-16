using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.Behaviours
{
    [BehaviourTemplate]
    public class MockSimpleAfterStartBehaviour : BehaviourAfterStart
    {  
        public override void Execute(IContainer arg)
        {
            arg.Resolve<MockPublisherProperties>().ManagedParameter = "Prefix";
        }

        public override BehaviourScope BehaviourScope { get; set; } = BehaviourScope.Service;
        public override BehaviourType BehaviourType { get; set; } = BehaviourType.AfterStart;
    }
}