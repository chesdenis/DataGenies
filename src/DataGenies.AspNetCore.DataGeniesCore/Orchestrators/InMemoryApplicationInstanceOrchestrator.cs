using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DataGenies.AspNetCore.DataGeniesCore.ApplicationTemplates;
using DataGenies.AspNetCore.DataGeniesCore.Models.Contexts;
using DataGenies.AspNetCore.DataGeniesCore.Scanners;

namespace DataGenies.AspNetCore.DataGeniesCore.Orchestrators
{
    public class InMemoryApplicationInstanceOrchestrator : IApplicationInstanceOrchestrator
    {
        private Dictionary<int, IStartable> _instancesInMemory { get; set; }

        private readonly ISchemaDataContext _schemaDataContext;
        private readonly IApplicationTemplatesScanner _applicationTemplatesScanner;

        public InMemoryApplicationInstanceOrchestrator(ISchemaDataContext schemaDataContext, IApplicationTemplatesScanner applicationTemplatesScanner)
        {
            _schemaDataContext = schemaDataContext;
            _applicationTemplatesScanner = applicationTemplatesScanner;

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
                this._schemaDataContext.ApplicationInstances.First(f => f.InstanceId == applicationInstanceId);
            var applicationTypeInfo = applicationInstanceInfo.Template;

            var matchTemplate = allTemplates.First(f => f.IsMatch(applicationTypeInfo));

            var templateType = Assembly.LoadFile(matchTemplate.AssemblyPath).GetType(matchTemplate.TypeName, true);

            var typeInstance = (IStartable) Activator.CreateInstance(templateType);

            _instancesInMemory[applicationInstanceId] = typeInstance;

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