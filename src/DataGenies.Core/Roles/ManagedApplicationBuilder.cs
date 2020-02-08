using System;
using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Configurators;
using DataGenies.Core.Converters;
using DataGenies.Core.Models;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;

namespace DataGenies.Core.Roles
{
    public class ManagedApplicationBuilder
    {
        private readonly ISchemaDataContext _schemaDataContext;
        private readonly IReceiverBuilder _receiverBuilder;
        private readonly IPublisherBuilder _publisherBuilder;
        private readonly IMqConfigurator _mqConfigurator;
       
        private Type _templateType;
        private ApplicationInstanceEntity _applicationInstanceEntity;
        
        private IEnumerable<IBehaviour> _behaviours = new List<IBehaviour>();
        private IEnumerable<IConverter> _converters = new List<IConverter>();

        public ManagedApplicationBuilder(
            ISchemaDataContext schemaDataContext, 
            IReceiverBuilder receiverBuilder, 
            IPublisherBuilder publisherBuilder,
            IMqConfigurator mqConfigurator)
        {
            _schemaDataContext = schemaDataContext;
            _receiverBuilder = receiverBuilder;
            _publisherBuilder = publisherBuilder;
            _mqConfigurator = mqConfigurator;
        }

        public ManagedApplicationBuilder UsingBehaviours(IEnumerable<IBehaviour> behaviours)
        {
            _behaviours = behaviours;
            return this;
        }

        public ManagedApplicationBuilder UsingConverters(IEnumerable<IConverter> converters)
        {
            _converters = converters;
            return this;
        }

        public ManagedApplicationBuilder UsingTemplateType(Type templateType)
        {
            this._templateType = templateType;
            return this;
        }

        public ManagedApplicationBuilder UsingApplicationInstance(ApplicationInstanceEntity applicationInstanceEntity)
        {
            this._applicationInstanceEntity = applicationInstanceEntity;
            return this;
        }

        public IManagedApplicationRole Build()
        {
            if (this._templateType.IsSubclassOf(typeof(ApplicationReceiverAndPublisherRole)))
            {
                var dataPublisherRole = BuildDataPublisherRole();
                var dataReceiverRole = BuildDataReceiverRole();
            
                var application =
                    (IRestartable) Activator.CreateInstance(this._templateType, dataReceiverRole, dataPublisherRole);

                if (application is IApplicationWithContext applicationWithState)
                {
                    Array.ForEach(_behaviours.ToArray(), b => b.SetContextContainer(applicationWithState.ContextContainer));
                }

                return new ManagedApplicationRole(application, _behaviours);
            }
            
            if (this._templateType.IsSubclassOf(typeof(ApplicationReceiverRole)))
            {
                var dataReceiverRole = BuildDataReceiverRole();
            
                var application =
                    (IRestartable) Activator.CreateInstance(this._templateType, dataReceiverRole);

                if (application is IApplicationWithContext applicationWithState)
                {
                    Array.ForEach(_behaviours.ToArray(), b => b.SetContextContainer(applicationWithState.ContextContainer));
                }
                
                return new ManagedApplicationRole(application, _behaviours);
            }

            if (this._templateType.IsSubclassOf(typeof(ApplicationPublisherRole)))
            {
                var dataPublisherRole = BuildDataPublisherRole();
            
                var application =
                    (IRestartable) Activator.CreateInstance(this._templateType, dataPublisherRole);

                if (application is IApplicationWithContext applicationWithState)
                {
                    Array.ForEach(_behaviours.ToArray(), b => b.SetContextContainer(applicationWithState.ContextContainer));
                }
                
                return new ManagedApplicationRole(application, _behaviours);
            }

            throw new NotImplementedException();
        }
 
        private DataReceiverRole BuildDataReceiverRole()
        {
            var receiver = this._receiverBuilder
                .WithQueue(this._applicationInstanceEntity.Name)
                .Build();

            ConfigureMqForReceiverRole();

            var dataReceiverRole = new DataReceiverRole(receiver, _converters);
            return dataReceiverRole;
        }

        private DataPublisherRole BuildDataPublisherRole()
        {
            var publisher = this._publisherBuilder
                .WithExchange(this._applicationInstanceEntity.Name)
                .Build();

            ConfigureMqForPublisherRole();

            var dataPublisherRole = new DataPublisherRole(publisher, _converters);
            return dataPublisherRole;
        }

        private void ConfigureMqForPublisherRole()
        {
            var relatedReceivers = _schemaDataContext.Bindings
                .Where(w => w.PublisherId == this._applicationInstanceEntity.Id);

            this._mqConfigurator.EnsureExchange(this._applicationInstanceEntity.Name);

            foreach (var receiver in relatedReceivers)
            {
                this._mqConfigurator.EnsureQueue(receiver.ReceiverApplicationInstanceEntity.Name, this._applicationInstanceEntity.Name,
                    $"{receiver.ReceiverRoutingKey}");
            }
        }

        private void ConfigureMqForReceiverRole()
        {
            var relatedPublishers = _schemaDataContext.Bindings
                .Where(w => w.ReceiverId == this._applicationInstanceEntity.Id);

            foreach (var publisher in relatedPublishers)
            {
                this._mqConfigurator.EnsureExchange(publisher.PublisherApplicationInstanceEntity.Name);
                this._mqConfigurator.EnsureQueue(
                    this._applicationInstanceEntity.Name, 
                    publisher.PublisherApplicationInstanceEntity.Name,
                    publisher.ReceiverRoutingKey);
            }
        }
    }
}