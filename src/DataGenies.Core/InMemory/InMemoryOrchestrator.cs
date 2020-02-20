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

namespace DataGenies.Core.InMemory
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
            return this._instancesInMemory[applicationInstanceId].Container;
        }

        public IQueryable<ApplicationInstanceEntity> GetDeployed()
        {
            return this._instancesInMemory.Keys.Select(s=>this._instancesInMemory[s])
                .Where(w=>w.State == ServiceState.Deployed)
        }

        public IQueryable<ApplicationInstanceEntity> GetStarted()
        {
            throw new NotImplementedException();
        }

        public IQueryable<ApplicationInstanceEntity> GetStopped()
        {
            throw new NotImplementedException();
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

            var behaviours = applicationInstanceInfo.Behaviours
                .Select(
                    s =>
                    {
                        var type = this._behaviourTemplatesScanner.FindType(s.TemplateEntity);
                        var instance = Activator.CreateInstance(type);

                        if (type.IsSubclassOf(typeof(BehaviourTemplate)))
                        {
                            var templateInstance = (BehaviourTemplate)instance;
                            templateInstance.BehaviourScope = s.BehaviourScope;
                            templateInstance.BehaviourType = s.BehaviourType;
                            return templateInstance;
                        }

                        return null;
                    }).Where(s=> s != null).ToArray();
            
            var wrapperBehaviours = applicationInstanceInfo.Behaviours
                .Select(
                    s =>
                    {
                        var type = this._behaviourTemplatesScanner.FindType(s.TemplateEntity);
                        var instance = Activator.CreateInstance(type);

                        if (type.IsSubclassOf(typeof(WrapperBehaviourTemplate)))
                        {
                            var templateInstance = (WrapperBehaviourTemplate)instance;
                            templateInstance.BehaviourScope = s.BehaviourScope;
                            return templateInstance;
                        }

                        return null;
                    }).Where(s=> s != null).ToArray();
             
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