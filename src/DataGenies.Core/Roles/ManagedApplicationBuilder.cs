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
        private ApplicationInstance _applicationInstance;

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

        public ManagedApplicationBuilder UsingTemplateType(Type templateType)
        {
            this._templateType = templateType;
            return this;
        }

        public ManagedApplicationBuilder UsingApplicationInstance(ApplicationInstance applicationInstance)
        {
            this._applicationInstance = applicationInstance;
            return this;
        }

        public IManagedApplicationRole Build()
        {
            var templateBehaviours = this.GetTemplateBehaviours(this._templateType);
            var templateConverters = this.GetTemplateBehaviours(this._templateType);
            
            if (this._templateType.IsSubclassOf(typeof(ApplicationReceiverAndPublisherRole)))
            {
                var dataPublisherRole = BuildDataPublisherRole();
                var dataReceiverRole = BuildDataReceiverRole();
            
                var application =
                    (IRestartable) Activator.CreateInstance(this._templateType, dataReceiverRole, dataPublisherRole);

               return new ManagedApplicationRole(application, templateBehaviours);
            }
            
            if (this._templateType.IsSubclassOf(typeof(ApplicationReceiverRole)))
            {
                var dataReceiverRole = BuildDataReceiverRole();
            
                var application =
                    (IRestartable) Activator.CreateInstance(this._templateType, dataReceiverRole);

                return new ManagedApplicationRole(application, templateBehaviours);
            }

            if (this._templateType.IsSubclassOf(typeof(ApplicationPublisherRole)))
            {
                var dataPublisherRole = BuildDataPublisherRole();
            
                var application =
                    (IRestartable) Activator.CreateInstance(this._templateType, dataPublisherRole);

                return new ManagedApplicationRole(application, templateBehaviours);
            }

            throw new NotImplementedException();
        }

        private IEnumerable<IBehaviour> GetTemplateBehaviours(Type templateType)
        {
            throw new NotImplementedException();
        }

        private DataReceiverRole BuildDataReceiverRole()
        {
            var receiver = this._receiverBuilder
                .WithQueue(this._applicationInstance.Name)
                .Build();

            ConfigureMqForReceiverRole();

            var dataReceiverRole = new DataReceiverRole(receiver, new List<IConverter>());
            return dataReceiverRole;
        }

        private DataPublisherRole BuildDataPublisherRole()
        {
            var publisher = this._publisherBuilder
                .WithExchange(this._applicationInstance.Name)
                .Build();

            ConfigureMqForPublisherRole();

            var dataPublisherRole = new DataPublisherRole(publisher, new List<IConverter>());
            return dataPublisherRole;
        }

        private void ConfigureMqForPublisherRole()
        {
            var relatedReceivers = _schemaDataContext.Bindings
                .Where(w => w.PublisherId == this._applicationInstance.Id);

            this._mqConfigurator.EnsureExchange(this._applicationInstance.Name);

            foreach (var receiver in relatedReceivers)
            {
                this._mqConfigurator.EnsureQueue(receiver.ReceiverApplicationInstance.Name, this._applicationInstance.Name,
                    $"{receiver.ReceiverRoutingKey}");
            }
        }

        private void ConfigureMqForReceiverRole()
        {
            var relatedPublishers = _schemaDataContext.Bindings
                .Where(w => w.ReceiverId == this._applicationInstance.Id);

            foreach (var publisher in relatedPublishers)
            {
                this._mqConfigurator.EnsureExchange(publisher.PublisherApplicationInstance.Name);
                this._mqConfigurator.EnsureQueue(
                    this._applicationInstance.Name, 
                    publisher.PublisherApplicationInstance.Name,
                    publisher.ReceiverRoutingKey);
            }
        }
    }
}