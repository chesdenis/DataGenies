using DataGenies.Core.Containers;
using DataGenies.Core.Roles;

namespace DataGenies.Core.Tests.Integration.Mocks.Abstractions
{
    public abstract class MockBasicAppTemplatePublisherWithState : ApplicationPublisherRole, IApplicationWithStateContainer
    {
        public IStateContainer StateContainer { get; set; } = new StateContainer();
        
        protected MockBasicAppTemplatePublisherWithState(DataPublisherRole publisherRole) 
            : base(publisherRole)
        {
        }
    }
}