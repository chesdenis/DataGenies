using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Models;
using DataGenies.Core.Orchestrators;
using DataGenies.Core.Scanners;
using DataGenies.Core.Services;
using DataGenies.Core.Wrappers;
using Microsoft.EntityFrameworkCore.Internal;

namespace DataGenies.InMemory
{
    public class InMemoryOrchestrator : IOrchestrator
    {
        private Dictionary<int, IManagedService> _instancesInMemory { get; set; }

        private readonly ISchemaDataContext _schemaDataContext;
        private readonly IApplicationTemplatesScanner _applicationTemplatesScanner;
        private readonly IBehaviourTemplatesScanner _behaviourTemplatesScanner;
       
        private readonly ManagedServiceBuilder _managedServiceBuilder;
        
        public InMemoryOrchestrator(ISchemaDataContext schemaDataContext,
            IApplicationTemplatesScanner applicationTemplatesScanner,
            IBehaviourTemplatesScanner behaviourTemplatesScanner,
            ManagedServiceBuilder managedServiceBuilder)
        {
            _schemaDataContext = schemaDataContext;
            
            _applicationTemplatesScanner = applicationTemplatesScanner;
            _behaviourTemplatesScanner = behaviourTemplatesScanner;
        
            _managedServiceBuilder = managedServiceBuilder;
           
            _instancesInMemory = new Dictionary<int, IManagedService>(); 
        }

        public IContainer GetApplicationInstanceContainer(int applicationInstanceId)
        {
            var applicationInstance = this._instancesInMemory[applicationInstanceId];
            if (applicationInstance is IManagedServiceWithContainer applicationInstanceWithContainer)
            {
                return applicationInstanceWithContainer.Container;
            }

            throw new NotSupportedException();
        }

        public Task PrepareTemplatePackage(int applicationInstanceId)
        {
            return Task.CompletedTask;
        }

        public Task Deploy(int applicationInstanceId)
        {
            var applicationInstanceInfo =
                this._schemaDataContext.ApplicationInstances.First(f => f.Id == applicationInstanceId);

            var templateType = this._applicationTemplatesScanner.FindType(applicationInstanceInfo.TemplateEntity);

            var behavioursTypes = applicationInstanceInfo.Behaviours
                .Select(s => this._behaviourTemplatesScanner.FindType(s.TemplateEntity))
                .ToArray();
            var allBehaviours = behavioursTypes.Select(Activator.CreateInstance).ToArray();
            var behaviours = allBehaviours.Where(w => w.GetType().IsSubclassOf(typeof(BehaviourTemplate)))
                .Select(s => (BehaviourTemplate)s).ToArray();
            var wrapperBehaviours = allBehaviours.Where(w => w.GetType().IsSubclassOf(typeof(WrapperBehaviourTemplate)))
                .Select(s => (WrapperBehaviourTemplate)s).ToArray();

            var managedApplication = this._managedServiceBuilder
                .UsingApplicationInstance(applicationInstanceInfo)
                .UsingTemplateType(templateType)
                .UsingBehaviours(behaviours)
                .UsingWrappersBehaviours(wrapperBehaviours)
                .Build();

            _instancesInMemory[applicationInstanceId] = managedApplication;

            return Task.CompletedTask;
        }

        public Task Start(int applicationInstanceId)
        {
            return Task.Run(() => _instancesInMemory[applicationInstanceId].Start());
        }

        public Task Stop(int applicationInstanceId)
        {
            return Task.Run(() => _instancesInMemory[applicationInstanceId].Stop());
        }

        public Task Remove(int applicationInstanceId)
        {
            return Task.Run(() =>
            {
                _instancesInMemory[applicationInstanceId].Stop();
                _instancesInMemory.Remove(applicationInstanceId);
            });
        }

        public Task Redeploy(int applicationInstanceId)
        {
            return Task.Run(() =>
            {
                _instancesInMemory[applicationInstanceId].Stop();
                _instancesInMemory.Remove(applicationInstanceId);

                Deploy(applicationInstanceId);
                Start(applicationInstanceId);
            });
        }

        public Task Restart(int applicationInstanceId)
        {
            return Task.Run(() =>
            {
                _instancesInMemory[applicationInstanceId].Stop();
                _instancesInMemory[applicationInstanceId].Start();
            });
        }

        public Task RestartAll()
        {
            foreach (var instanceInMemory in this._instancesInMemory.Keys)
            {
                Restart(instanceInMemory);
            }
            
            return  Task.CompletedTask;
        }

        public Task RemoveAll()
        {
            foreach (var instanceInMemory in this._instancesInMemory.Keys)
            {
                Stop(instanceInMemory);
            }
            _instancesInMemory.Clear();
            return  Task.CompletedTask;
        }
    }
}