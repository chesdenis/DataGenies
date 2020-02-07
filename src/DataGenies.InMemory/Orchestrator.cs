using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DataGenies.Core.Behaviours;
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
        private readonly IApplicationBehavioursScanner _applicationBehavioursScanner;
        private readonly ManagedApplicationBuilder _managedApplicationBuilder;
        
        public Orchestrator(ISchemaDataContext schemaDataContext,
            IApplicationTemplatesScanner applicationTemplatesScanner,
            IApplicationBehavioursScanner applicationBehavioursScanner,
            ManagedApplicationBuilder managedApplicationBuilder)
        {
            _schemaDataContext = schemaDataContext;
            _applicationTemplatesScanner = applicationTemplatesScanner;
            _applicationBehavioursScanner = applicationBehavioursScanner;
            _managedApplicationBuilder = managedApplicationBuilder;
           
            _instancesInMemory = new Dictionary<int, IRestartable>(); 
        }
        
        public Task PrepareTemplatePackage(int applicationInstanceId)
        {
            return Task.CompletedTask;
        }

        public Task Deploy(int applicationInstanceId)
        {
            var applicationInstanceInfo =
                this._schemaDataContext.ApplicationInstances.First(f => f.Id == applicationInstanceId);

            // get template type to build managed application
            var applicationTypeInfo = applicationInstanceInfo.Template;
            var allTemplates = this._applicationTemplatesScanner.ScanTemplates();
            var matchTemplate = allTemplates.First(f => f.IsMatch(applicationTypeInfo));
            var templateType = Assembly.LoadFile(matchTemplate.AssemblyPath).GetType(matchTemplate.Name, true);

            var assignedBehaviours = applicationInstanceInfo.Behaviours;
            var allBehaviours = this._applicationBehavioursScanner.ScanBehaviours();
            var matchedBehaviours = allBehaviours
                .Where(w => assignedBehaviours.Any(c => c.IsMatch(w)));
            var matchedBehaviourInstances = (IEnumerable<IBehaviour>)matchedBehaviours
                .Select(s => 
                    Activator.CreateInstance(
                        Assembly.LoadFile(s.AssemblyPath).GetType(s.Name, true)));
            
            var managedApplication = this._managedApplicationBuilder
                .UsingApplicationInstance(applicationInstanceInfo)
                .UsingTemplateType(templateType)
                .UsingBehaviours(matchedBehaviourInstances)
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