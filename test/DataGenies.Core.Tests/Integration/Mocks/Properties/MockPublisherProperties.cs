using System.Collections.Generic;

namespace DataGenies.Core.Tests.Integration.Mocks.Properties
{
    public class MockPublisherProperties
    {
        public string ManagedParameter { get; set; }
        public List<string> PublishedMessages { get; set; } = new List<string>();
    }
}