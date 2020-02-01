using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DataGenies.Core.Models.Contexts;
using DataGenies.Core.Orchestrators;
using DataGenies.Core.Roles;
using DataGenies.Core.Scanners;

namespace DataGenies.InMemory
{
    public class Orchestrator : IOrchestrator
    {
        private Dictionary<int, IStartable> _instancesInMemory { get; set; }

        private readonly ISchemaDataContext _schemaDataContext;
        private readonly IApplicationTemplatesScanner _applicationTemplatesScanner;
        private readonly MqBroker _mqBroker;

        public Orchestrator(ISchemaDataContext schemaDataContext,
            IApplicationTemplatesScanner applicationTemplatesScanner, MqBroker mqBroker)
        {
            _schemaDataContext = schemaDataContext;
            _applicationTemplatesScanner = applicationTemplatesScanner;
            _mqBroker = mqBroker;

            _instancesInMemory = new Dictionary<int, IStartable>(); 
        }
        
        public Task PrepareTemplatePackage(int applicationInstanceId)
        {
            return Task.CompletedTask;
        }

        public Task Deploy(int applicationInstanceId)
        {
            var allTemplates = this._applicationTemplatesScanner.ScanTemplates();
            var applicationInstanceInfo =
                this._schemaDataContext.ApplicationInstances.First(f => f.Id == applicationInstanceId);
            var applicationTypeInfo = applicationInstanceInfo.Template;

            var matchTemplate = allTemplates.First(f => f.IsMatch(applicationTypeInfo));

            var templateType = Assembly.LoadFile(matchTemplate.AssemblyPath).GetType(matchTemplate.Name, true);
            if (templateType.IsSubclassOf(typeof(ApplicationPublisherRole)))
            {
                var typeInstance = (IStartable) Activator.CreateInstance(templateType);

                _instancesInMemory[applicationInstanceId] = typeInstance;

            }

            return Task.CompletedTask;
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

        public Task SetupIncomingBinding(int applicationInstanceId)
        {
            throw new System.NotImplementedException();
        }

        public Task SetupOutcomingBinding(int applicationInstanceId)
        {
            throw new System.NotImplementedException();
        }
    }
}