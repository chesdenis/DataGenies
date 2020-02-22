using System;
using System.Collections.Generic;
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
            
            var container = new Container();
            
            container.Register<string>(this._applicationInstanceEntity.ParametersDictAsJson, "ParametersDictAsJson");
            container.Register<string>(this._applicationInstanceEntity.TemplateEntity.ConfigTemplateJson, "ConfigTemplateJson");

            var ctorArgs = new List<object>
            {
                container,
                publisher,
                receiver,
                _behaviourTemplates,
                _wrapperBehaviours
            };

            var managedService =
                (IManagedService) Activator.CreateInstance(this._templateType, ctorArgs.ToArray());
            
            managedService.ApplicationInstanceEntityId = this._applicationInstanceEntity.Id;
            managedService.State = ServiceState.Created;
                
            this._bindingConfigurator.ConfigureFor(this._applicationInstanceEntity.Id);

            foreach (var virtualBinding in managedService.GetVirtualBindings())
            {
                switch (virtualBinding.Scope)
                {
                    case VirtualBindingScope.Instance:
                    {
                        this._bindingConfigurator.ConfigureForInstanceScope(this._applicationInstanceEntity.Id, virtualBinding.InstanceName, virtualBinding.RoutingKey);
                    }
                        break;
                    case VirtualBindingScope.Template:
                    {
                        this._bindingConfigurator.ConfigureForTemplateScope(this._applicationInstanceEntity.Id, virtualBinding.InstanceName, virtualBinding.RoutingKey);
                    }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return managedService;
        }
    }
}