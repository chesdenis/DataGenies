using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Models;
using DataGenies.Core.Orchestrators;
using DataGenies.Core.Scanners;
using DataGenies.Core.Services;
using DataGenies.Core.Wrappers;

namespace DataGenies.InMemory
{
    public class InMemoryOrchestrator : IOrchestrator
    {
        private Dictionary<int, IManagedService> _instancesInMemory { get; set; }

        private readonly ISchemaDataContext _schemaDataContext;
        private readonly IApplicationTemplatesScanner _applicationTemplatesScanner;
        private readonly IApplicationBehavioursScanner _applicationBehavioursScanner;
       
        private readonly ManagedServiceBuilder _managedServiceBuilder;
        
        public InMemoryOrchestrator(ISchemaDataContext schemaDataContext,
            IApplicationTemplatesScanner applicationTemplatesScanner,
            IApplicationBehavioursScanner applicationBehavioursScanner,
            ManagedServiceBuilder managedServiceBuilder)
        {
            _schemaDataContext = schemaDataContext;
            _applicationTemplatesScanner = applicationTemplatesScanner;
            _applicationBehavioursScanner = applicationBehavioursScanner;
        
            _managedServiceBuilder = managedServiceBuilder;
           
            _instancesInMemory = new Dictionary<int, IManagedService>(); 
        }

        public IManagedService GetManagedApplicationInstance(int applicationInstanceId)
        {
            return this._instancesInMemory[applicationInstanceId];
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
            var behaviours =
                this._applicationBehavioursScanner.GetBehavioursInstances(applicationInstanceInfo.Behaviours).ToList();

            var basicBehaviours = behaviours
                .Where(w => 
                    w.BehaviourType == BehaviourType.AfterStart 
                    | 
                    w.BehaviourType == BehaviourType.BeforeStart)
                .OfType<IBasicBehaviour>();
            
            var wrapperBehaviours = behaviours
                .Where(w => 
                    w.BehaviourType == BehaviourType.Wrapper)
                .OfType<IWrapperBehaviour>();
            
            var onExceptionBehaviours = behaviours
                .Where(w => 
                    w.BehaviourType == BehaviourType.Wrapper)
                .OfType<IBehaviourOnException>();
          
            var managedApplication = this._managedServiceBuilder
                .UsingApplicationInstance(applicationInstanceInfo)
                .UsingTemplateType(templateType)
                .UsingBasicBehaviours(basicBehaviours)
                .UsingWrappersBehaviours(wrapperBehaviours)
                .UsingOnExceptionBehaviours(onExceptionBehaviours)
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