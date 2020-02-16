using System.Collections.Generic;

namespace DataGenies.Core.Tests.Integration.Mocks.Properties
{
    public class MockBehaviourProperties
    {
        public List<string> HandledMessages { get; set; } = new List<string>();
    }

    public class MockReceiverProperties
    {
        public List<string> ReceivedMessages { get; set; } = new List<string>();
    }

    public class MockPublisherProperties
    {
        public string ManagedParameter { get; set; }
        public List<string> PublishedMessages { get; set; } = new List<string>();
    }
}