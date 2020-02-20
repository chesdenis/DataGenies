using System.Collections.Generic;

namespace DataGenies.Core.Tests.Integration.Mocks.Properties
{
    public class MockReceiverProperties
    {
        public string ManagedParameter { get; set; }
        public List<string> ReceivedMessages { get; set; } = new List<string>();
    }
}