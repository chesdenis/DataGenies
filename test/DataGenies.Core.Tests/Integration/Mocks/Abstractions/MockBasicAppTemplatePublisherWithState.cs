using DataGenies.Core.Roles;

namespace DataGenies.Core.Tests.Integration.Mocks.Abstractions
{
    public abstract class MockBasicAppTemplatePublisherWithState : ApplicationPublisherRole, IApplicationWithProperties
    {
        public IApplicationProperties Properties { get; set; }
        
        protected MockBasicAppTemplatePublisherWithState(DataPublisherRole publisherRole) 
            : base(publisherRole)
        {
        }
    }
}