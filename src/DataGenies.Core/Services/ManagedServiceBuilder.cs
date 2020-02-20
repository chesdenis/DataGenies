using System;
using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Configurators;
using DataGenies.Core.Containers;
using DataGenies.Core.Models;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;

namespace DataGenies.Core.Services
{
    public class ManagedServiceBuilder
    {
        private readonly IReceiverBuilder _receiverBuilder;
        private readonly IPublisherBuilder _publisherBuilder;
        private readonly IBindingConfigurator _bindingConfigurator;
       
        private Type _templateType;
        private ApplicationInstanceEntity _applicationInstanceEntity;
        
        private IEnumerable<BehaviourTemplate> _behaviourTemplates { get; set; }
        private IEnumerable<WrapperBehaviourTemplate> _wrapperBehaviours { get; set; }
        
        public ManagedServiceBuilder(
            IReceiverBuilder receiverBuilder, 
            IPublisherBuilder publisherBuilder,
            IBindingConfigurator bindingConfigurator)
        {
            _receiverBuilder = receiverBuilder;
            _publisherBuilder = publisherBuilder;
            _bindingConfigurator = bindingConfigurator;
        }

        public ManagedServiceBuilder UsingBehaviours(IEnumerable<BehaviourTemplate> behaviourTemplates)
        {
            _behaviourTemplates = behaviourTemplates;
            return this;
        }
        
        public ManagedServiceBuilder UsingWrappersBehaviours(IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours)
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
            var receiver = this._receiverBuilder
                .WithQueue(this._applicationInstanceEntity.Name)
                .Build();
                
            var publisher = this._publisherBuilder
                .WithExchange(this._applicationInstanceEntity.Name)
                .Build();

            var ctorArgs = new List<Object>();
               
            if (this._templateType.IsSubclassOf(typeof(ManagedCommunicableServiceWithContainer)))
            {
                ctorArgs.Add(new Container());
            }

            ctorArgs.Add(publisher);
            ctorArgs.Add(receiver);
            ctorArgs.Add(_behaviourTemplates);
            ctorArgs.Add(_wrapperBehaviours);
                 
            var managedService =
                (IManagedService) Activator.CreateInstance(this._templateType, ctorArgs.ToArray());
                
            this._bindingConfigurator.ConfigureFor(managedService, this._applicationInstanceEntity);

            return managedService;
        }
    }
}