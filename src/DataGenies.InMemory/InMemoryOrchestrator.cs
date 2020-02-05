using System.Threading.Tasks;
using DataGenies.Core.Models;
using DataGenies.Core.Orchestrators;

namespace DataGenies.InMemory
{
    public class InMemoryOrchestrator : IOrchestrator
    {
        private readonly ISchemaDataContext _schemaDataContext;
        private readonly SchemaDataBuilder _schemaDataBuilder;

        public InMemoryOrchestrator(ISchemaDataContext schemaDataContext, SchemaDataBuilder schemaDataBuilder)
        {
            this._schemaDataContext = schemaDataContext;
            _schemaDataBuilder = schemaDataBuilder;
        }
        
        public Task PrepareTemplatePackage(int applicationInstanceId)
        {
            throw new System.NotImplementedException();
        }

        public Task Deploy(int applicationInstanceId)
        {
            throw new System.NotImplementedException();
        }

        public Task Remove(int applicationInstanceId)
        {
            throw new System.NotImplementedException();
        }

        public Task Redeploy(int applicationInstanceId)
        {
            throw new System.NotImplementedException();
        }

        public Task Restart(int applicationInstanceId)
        {
            throw new System.NotImplementedException();
        }

        public Task RestartAll()
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveAll()
        {
            throw new System.NotImplementedException();
        }
    }
}