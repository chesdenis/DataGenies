using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DataGenies.Core.Models;
using DataGenies.Core.Orchestrators;
using DataGenies.Core.Roles;
using DataGenies.Core.Scanners;

namespace DataGenies.InMemory
{
    public class Orchestrator : IOrchestrator
    {
        private Dictionary<int, IRestartable> _instancesInMemory { get; set; }

        private readonly ISchemaDataContext _schemaDataContext;
        private readonly IApplicationTemplatesScanner _applicationTemplatesScanner;
        private readonly ManagedApplicationBuilder _managedApplicationBuilder;
        
        public Orchestrator(ISchemaDataContext schemaDataContext,
            IApplicationTemplatesScanner applicationTemplatesScanner,
            ManagedApplicationBuilder managedApplicationBuilder)
        {
            _schemaDataContext = schemaDataContext;
            _applicationTemplatesScanner = applicationTemplatesScanner;
            _managedApplicationBuilder = managedApplicationBuilder;
           
            _instancesInMemory = new Dictionary<int, IRestartable>(); 
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

            var managedApplication = this._managedApplicationBuilder
                .UsingApplicationInstance(applicationInstanceInfo)
                .UsingTemplateType(templateType)
                .Build(); 
            
            _instancesInMemory[applicationInstanceId] = managedApplication;
            
            _instancesInMemory[applicationInstanceId].Start();
            return Task.CompletedTask;
        }
        
        public Task Remove(int applicationInstanceId)
        {
            _instancesInMemory[applicationInstanceId].Stop();
            _instancesInMemory.Remove(applicationInstanceId);
            
            return  Task.CompletedTask;
        }

        public Task Redeploy(int applicationInstanceId)
        {
            _instancesInMemory[applicationInstanceId].Stop();
            _instancesInMemory.Remove(applicationInstanceId);

            Deploy(applicationInstanceId);
            
            return  Task.CompletedTask;
        }

        public Task Restart(int applicationInstanceId)
        {
            _instancesInMemory[applicationInstanceId].Stop();
            _instancesInMemory[applicationInstanceId].Start();
            
            return  Task.CompletedTask;
        }

        public Task RestartAll()
        {
            foreach (var instanceInMemory in this._instancesInMemory.Keys)
            {
                _instancesInMemory[instanceInMemory].Stop();
                _instancesInMemory[instanceInMemory].Start();
            }
            
            return  Task.CompletedTask;
        }

        public Task RemoveAll()
        {
            _instancesInMemory.Clear();
            
            return  Task.CompletedTask;
        }
    }
}