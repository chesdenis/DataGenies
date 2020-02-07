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
    public class InMemoryOrchestrator : IOrchestrator
    {
        private Dictionary<int, IRestartable> _instancesInMemory { get; set; }

        private readonly ISchemaDataContext _schemaDataContext;
        private readonly IApplicationTemplatesScanner _applicationTemplatesScanner;
        private readonly IApplicationBehavioursScanner _applicationBehavioursScanner;
        private readonly ManagedApplicationBuilder _managedApplicationBuilder;
        
        public InMemoryOrchestrator(ISchemaDataContext schemaDataContext,
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
            
            var templateType = this._applicationTemplatesScanner.FindType(applicationInstanceInfo.Template);
            var behaviours =
                this._applicationBehavioursScanner.GetBehavioursInstances(applicationInstanceInfo.Behaviours);
            
            var managedApplication = this._managedApplicationBuilder
                .UsingApplicationInstance(applicationInstanceInfo)
                .UsingTemplateType(templateType)
                .UsingBehaviours(behaviours)
                .Build(); 
            
            _instancesInMemory[applicationInstanceId] = managedApplication;

            return Task.CompletedTask;
        }

        public Task Start(int applicationInstanceId)
        {
            _instancesInMemory[applicationInstanceId].Start();
            return Task.CompletedTask;
        }

        public Task Stop(int applicationInstanceId)
        {
            _instancesInMemory[applicationInstanceId].Stop();
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

            Start(applicationInstanceId);
            
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