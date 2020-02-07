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
        
        private readonly List<IBehaviour> _behaviours = new List<IBehaviour>();
        private readonly List<IConverter> _converters = new List<IConverter>();

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
            _behaviours.AddRange(behaviours);
            return this;
        }

        public ManagedApplicationBuilder UsingConverters(IEnumerable<IConverter> converters)
        {
            _converters.AddRange(converters);
            return this;
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
            if (this._templateType.IsSubclassOf(typeof(ApplicationReceiverAndPublisherRole)))
            {
                var dataPublisherRole = BuildDataPublisherRole();
                var dataReceiverRole = BuildDataReceiverRole();
            
                var application =
                    (IRestartable) Activator.CreateInstance(this._templateType, dataReceiverRole, dataPublisherRole);

                if (application is IApplicationWithStateContainer applicationWithState)
                {
                    Array.ForEach(_behaviours.ToArray(), b => b.SetStateContainer(applicationWithState.StateContainer));
                }

                return new ManagedApplicationRole(application, _behaviours);
            }
            
            if (this._templateType.IsSubclassOf(typeof(ApplicationReceiverRole)))
            {
                var dataReceiverRole = BuildDataReceiverRole();
            
                var application =
                    (IRestartable) Activator.CreateInstance(this._templateType, dataReceiverRole);

                if (application is IApplicationWithStateContainer applicationWithState)
                {
                    Array.ForEach(_behaviours.ToArray(), b => b.SetStateContainer(applicationWithState.StateContainer));
                }
                
                return new ManagedApplicationRole(application, _behaviours);
            }

            if (this._templateType.IsSubclassOf(typeof(ApplicationPublisherRole)))
            {
                var dataPublisherRole = BuildDataPublisherRole();
            
                var application =
                    (IRestartable) Activator.CreateInstance(this._templateType, dataPublisherRole);

                if (application is IApplicationWithStateContainer applicationWithState)
                {
                    Array.ForEach(_behaviours.ToArray(), b => b.SetStateContainer(applicationWithState.StateContainer));
                }
                
                return new ManagedApplicationRole(application, _behaviours);
            }

            throw new NotImplementedException();
        }
 
        private DataReceiverRole BuildDataReceiverRole()
        {
            var receiver = this._receiverBuilder
                .WithQueue(this._applicationInstance.Name)
                .Build();

            ConfigureMqForReceiverRole();

            var dataReceiverRole = new DataReceiverRole(receiver, _converters);
            return dataReceiverRole;
        }

        private DataPublisherRole BuildDataPublisherRole()
        {
            var publisher = this._publisherBuilder
                .WithExchange(this._applicationInstance.Name)
                .Build();

            ConfigureMqForPublisherRole();

            var dataPublisherRole = new DataPublisherRole(publisher, _converters);
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