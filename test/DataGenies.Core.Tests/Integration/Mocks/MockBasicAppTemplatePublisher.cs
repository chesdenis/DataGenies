using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using DataGenies.Core.Roles;

namespace DataGenies.Core.Tests.Integration.Mocks
{
    public abstract class MockBasicAppTemplatePublisher : ApplicationPublisherRole
    {
        public readonly List<byte[]> State = new List<byte[]>();

        protected MockBasicAppTemplatePublisher(DataPublisherRole publisherRole) : base(publisherRole)
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