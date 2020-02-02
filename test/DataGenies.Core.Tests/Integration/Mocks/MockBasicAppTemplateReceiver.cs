using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenies.Core.Roles;

namespace DataGenies.Core.Tests.Integration.Mocks
{
    public abstract class MockBasicAppTemplateReceiver : ApplicationReceiverRole
    {
        public readonly List<byte[]> State = new List<byte[]>();
        
        protected MockBasicAppTemplateReceiver(DataReceiverRole receiverRole) : base(receiverRole)
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