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
        private readonly IApplicationTemplatesScanner applicationTemplatesScanner;
        private readonly IBehaviourTemplatesScanner behaviourTemplatesScanner;

        private readonly ManagedServiceBuilder managedServiceBuilder;

        private readonly ISchemaDataContext schemaDataContext;

        public InMemoryOrchestrator(
            ISchemaDataContext schemaDataContext,
            IApplicationTemplatesScanner applicationTemplatesScanner,
            IBehaviourTemplatesScanner behaviourTemplatesScanner,
            ManagedServiceBuilder managedServiceBuilder)
        {
            this.schemaDataContext = schemaDataContext;

            this.applicationTemplatesScanner = applicationTemplatesScanner;
            this.behaviourTemplatesScanner = behaviourTemplatesScanner;

            this.managedServiceBuilder = managedServiceBuilder;

            this.InstancesInMemory = new Dictionary<int, IManagedService>();
        }

        private Dictionary<int, IManagedService> InstancesInMemory { get; }

        public IQueryable<ApplicationInstanceEntity> GetDeployed()
        {
            throw new NotImplementedException();
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
                this.schemaDataContext.ApplicationInstances.First(f => f.Id == applicationInstanceId);

            var templateType = this.applicationTemplatesScanner.FindType(applicationInstanceInfo.TemplateEntity);

            var behaviours = applicationInstanceInfo.Behaviours
                .Select(
                    s =>
                    {
                        var type = this.behaviourTemplatesScanner.FindType(s.TemplateEntity);
                        var instance = Activator.CreateInstance(type);

                        if (type.IsSubclassOf(typeof(BehaviourTemplate)))
                        {
                            var templateInstance = (BehaviourTemplate)instance;
                            templateInstance.BehaviourScope = s.BehaviourScope;
                            templateInstance.BehaviourType = s.BehaviourType;
                            return templateInstance;
                        }

                        return null;
                    }).Where(s => s != null).ToArray();

            var wrapperBehaviours = applicationInstanceInfo.Behaviours
                .Select(
                    s =>
                    {
                        var type = this.behaviourTemplatesScanner.FindType(s.TemplateEntity);
                        var instance = Activator.CreateInstance(type);

                        if (type.IsSubclassOf(typeof(WrapperBehaviourTemplate)))
                        {
                            var templateInstance = (WrapperBehaviourTemplate)instance;
                            templateInstance.BehaviourScope = s.BehaviourScope;
                            return templateInstance;
                        }

                        return null;
                    }).Where(s => s != null).ToArray();

            var managedApplication = this.managedServiceBuilder
                .UsingApplicationInstance(applicationInstanceInfo)
                .UsingTemplateType(templateType)
                .UsingBehaviours(behaviours)
                .UsingWrappersBehaviours(wrapperBehaviours)
                .Build();

            this.InstancesInMemory[applicationInstanceId] = managedApplication;

            return Task.CompletedTask;
        }

        public Task Start(int applicationInstanceId)
        {
            return Task.Run(() => this.InstancesInMemory[applicationInstanceId].Start());
        }

        public Task Stop(int applicationInstanceId)
        {
            return Task.Run(() => this.InstancesInMemory[applicationInstanceId].Stop());
        }

        public Task Remove(int applicationInstanceId)
        {
            return Task.Run(
                () =>
                {
                    this.InstancesInMemory[applicationInstanceId].Stop();
                    this.InstancesInMemory.Remove(applicationInstanceId);
                });
        }

        public Task Redeploy(int applicationInstanceId)
        {
            return Task.Run(
                () =>
                {
                    this.InstancesInMemory[applicationInstanceId].Stop();
                    this.InstancesInMemory.Remove(applicationInstanceId);

                    this.Deploy(applicationInstanceId);
                    this.Start(applicationInstanceId);
                });
        }

        public Task Restart(int applicationInstanceId)
        {
            return Task.Run(
                () =>
                {
                    this.InstancesInMemory[applicationInstanceId].Stop();
                    this.InstancesInMemory[applicationInstanceId].Start();
                });
        }

        public Task RestartAll()
        {
            foreach (var instanceInMemory in this.InstancesInMemory.Keys)
            {
                this.Restart(instanceInMemory);
            }

            return Task.CompletedTask;
        }

        public Task RemoveAll()
        {
            foreach (var instanceInMemory in this.InstancesInMemory.Keys)
            {
                this.Stop(instanceInMemory);
            }

            this.InstancesInMemory.Clear();
            return Task.CompletedTask;
        }

        public IContainer GetApplicationInstanceContainer(int applicationInstanceId)
        {
            return this.InstancesInMemory[applicationInstanceId].Container;
        }
    }
}