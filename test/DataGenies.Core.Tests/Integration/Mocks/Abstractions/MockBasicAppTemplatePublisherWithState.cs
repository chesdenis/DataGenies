using DataGenies.Core.Containers;
using DataGenies.Core.Roles;

namespace DataGenies.Core.Tests.Integration.Mocks.Abstractions
{
    public abstract class MockBasicAppTemplatePublisherWithState : ApplicationPublisherRole, IApplicationWithContext
    {
        public IContainer ContextContainer { get; set; } = new Container();
        
        protected MockBasicAppTemplatePublisherWithState(DataPublisherRole publisherRole) 
            : base(publisherRole)
        {
        }
    }
}