using System;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Configurators;
using DataGenies.Core.Containers;
using DataGenies.Core.Models;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Wrappers;

namespace DataGenies.Core.Services
{
    public class ManagedServiceBuilder
    {
        private readonly IReceiverBuilder _receiverBuilder;
        private readonly IPublisherBuilder _publisherBuilder;
        private readonly IBindingConfigurator _bindingConfigurator;
       
        private Type _templateType;
        private ApplicationInstanceEntity _applicationInstanceEntity;
        
        private IEnumerable<IBasicBehaviour> _basicBehaviours = new List<IBasicBehaviour>();
        private IEnumerable<IBehaviourOnException> _behaviourOnExceptions = new List<IBehaviourOnException>();
        private IEnumerable<IWrapperBehaviour> _wrapperBehaviours = new List<IWrapperBehaviour>();
       
        public ManagedServiceBuilder(
            IReceiverBuilder receiverBuilder, 
            IPublisherBuilder publisherBuilder,
            IBindingConfigurator bindingConfigurator)
        {
            _receiverBuilder = receiverBuilder;
            _publisherBuilder = publisherBuilder;
            _bindingConfigurator = bindingConfigurator;
        }

        public ManagedServiceBuilder UsingBasicBehaviours(IEnumerable<IBasicBehaviour> basicBehaviours)
        {
            _basicBehaviours = basicBehaviours;
            return this;
        }

        public ManagedServiceBuilder UsingOnExceptionBehaviours(
            IEnumerable<IBehaviourOnException> behaviourOnExceptions)
        {
            _behaviourOnExceptions = behaviourOnExceptions;
            return this;
        }
        
        public ManagedServiceBuilder UsingWrappersBehaviours(
            IEnumerable<IWrapperBehaviour> wrapperBehaviours)
        {
            _wrapperBehaviours = wrapperBehaviours;
            return this;
        }
        
        public ManagedServiceBuilder UsingTemplateType(Type templateType)
        {
            this._templateType = templateType;
            return this;
        }

        public ManagedServiceBuilder UsingApplicationInstance(ApplicationInstanceEntity applicationInstanceEntity)
        {
            this._applicationInstanceEntity = applicationInstanceEntity;
            return this;
        }

        public IManagedService Build()
        {
            if (this._templateType.IsSubclassOf(typeof(ManagedReceiverAndPublisherService)) 
                ||
                this._templateType.IsSubclassOf(typeof(ManagedReceiverAndPublisherServiceWithContainer)))
            {
                var receiver = this._receiverBuilder
                    .WithQueue(this._applicationInstanceEntity.Name)
                    .Build();
                
                var publisher = this._publisherBuilder
                    .WithExchange(this._applicationInstanceEntity.Name)
                    .Build();

                var ctorArgs = new List<Object>();
               
                if (this._templateType.IsSubclassOf(typeof(ManagedReceiverAndPublisherServiceWithContainer)))
                {
                    ctorArgs.Add(new Container());
                }

                ctorArgs.Add(publisher);
                ctorArgs.Add(receiver);
                ctorArgs.Add(_basicBehaviours);
                ctorArgs.Add(_behaviourOnExceptions);
                ctorArgs.Add(_wrapperBehaviours);
                 
                var managedService =
                    (IManagedService) Activator.CreateInstance(this._templateType, ctorArgs);
                
                this._bindingConfigurator.ConfigureFor(managedService, this._applicationInstanceEntity);

                return managedService;
            }
            
            if (this._templateType.IsSubclassOf(typeof(ManagedReceiverService))
            ||
            this._templateType.IsSubclassOf(typeof(ManagedReceiverServiceWithContainer)))
            {
                var receiver = this._receiverBuilder
                    .WithQueue(this._applicationInstanceEntity.Name)
                    .Build();
                
                var ctorArgs = new List<Object>();
                
                if (this._templateType.IsSubclassOf(typeof(ManagedReceiverServiceWithContainer)))
                {
                    ctorArgs.Add(new Container());
                }
                
                ctorArgs.Add(receiver);
                ctorArgs.Add(_basicBehaviours);
                ctorArgs.Add(_behaviourOnExceptions);
                ctorArgs.Add(_wrapperBehaviours);
                
                var managedService =
                    (IManagedService) Activator.CreateInstance(this._templateType, ctorArgs);
                
                this._bindingConfigurator.ConfigureFor(managedService, this._applicationInstanceEntity);

                return managedService;
            }

            if (this._templateType.IsSubclassOf(typeof(ManagedPublisherService)) 
                ||
                this._templateType.IsSubclassOf(typeof(ManagedPublisherServiceWithContainer)) )
            {
                var publisher = this._publisherBuilder
                    .WithExchange(this._applicationInstanceEntity.Name)
                    .Build();
                
                var ctorArgs = new List<Object>();
                
                if (this._templateType.IsSubclassOf(typeof(ManagedPublisherServiceWithContainer)))
                {
                    ctorArgs.Add(new Container());
                }
                
                ctorArgs.Add(publisher);
                ctorArgs.Add(_basicBehaviours);
                ctorArgs.Add(_behaviourOnExceptions);
                ctorArgs.Add(_wrapperBehaviours);
                
                var managedService =
                    (IManagedService) Activator.CreateInstance(this._templateType, ctorArgs);
                
                this._bindingConfigurator.ConfigureFor(managedService, this._applicationInstanceEntity);

                return managedService;
            }

            throw new NotImplementedException();
        }
  
       
    }
}