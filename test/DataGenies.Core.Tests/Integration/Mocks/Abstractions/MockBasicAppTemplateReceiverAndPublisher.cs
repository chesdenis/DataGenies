using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenies.Core.Roles;

namespace DataGenies.Core.Tests.Integration.Mocks.Abstractions
{
    public abstract class MockBasicAppTemplateReceiverAndPublisher : ApplicationReceiverAndPublisherRole
    {
        public readonly List<byte[]> State = new List<byte[]>();


        protected MockBasicAppTemplateReceiverAndPublisher(DataReceiverRole receiverRole,
            DataPublisherRole publisherRole) : base(receiverRole, publisherRole)
        {
        }
        
        public int GetMessagesCountInState()
        {
            return State.Count;
        }

        public string GetLastMessageAsString()
        {
            return Encoding.UTF8.GetString(State.Last());
        }
    }
}