using DataGenies.Core.Containers;
using DataGenies.Core.Roles;

namespace DataGenies.Core.Tests.Integration.Mocks.Properties
{
    public class MockSampleProperties : IApplicationProperties
    {
        public string PropertyA { get; set; }

        public string PropertyB { get; set; }
    }
}