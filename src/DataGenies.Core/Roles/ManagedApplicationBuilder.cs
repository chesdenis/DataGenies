using System;
using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Behaviours;
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
        private Type _templateType;
        private ApplicationInstance _applicationInstance;

        public ManagedApplicationBuilder(
            ISchemaDataContext schemaDataContext, 
            IReceiverBuilder receiverBuilder, 
            IPublisherBuilder publisherBuilder)
        {
            _schemaDataContext = schemaDataContext;
            _receiverBuilder = receiverBuilder;
            _publisherBuilder = publisherBuilder;
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

        public IRestartable Build()
        {
            if (this._templateType.IsSubclassOf(typeof(ApplicationReceiverAndPublisherRole)))
            {
                return BuildUsingReceiverAndPublisherRole();
            }
            
            if (this._templateType.IsSubclassOf(typeof(ApplicationReceiverRole)))
            {
                return BuildUsingReceiverRole();
            }

            if (this._templateType.IsSubclassOf(typeof(ApplicationPublisherRole)))
            {
                return BuildUsingPublisherRole();
            }

            throw new NotImplementedException();
        }

        private IRestartable BuildUsingReceiverAndPublisherRole()
        {
            var relatedPublishers = _schemaDataContext.Bindings
                .Where(w => w.ReceiverId == this._applicationInstance.Id)
                .Select(s => s.PublisherApplicationInstance.Name);
                
            var receiver = this._receiverBuilder
                .WithQueue(this._applicationInstance.Name)
                .WithRoutingKeys(relatedPublishers)
                .Build();

            var dataReceiverRole = new DataReceiverRole(receiver, new List<IConverter>());
                
            var publisher = this._publisherBuilder
                .WithExchange(this._applicationInstance.Name)
                .Build();

            var dataPublisherRole = new DataPublisherRole(publisher, new List<IConverter>());

            var application = (IRestartable)Activator.CreateInstance(this._templateType, dataReceiverRole, dataPublisherRole);

            var managedApplication = new ManagedApplicationRole(application, new List<IBehaviour>());
                
            return managedApplication;
        }

        private IRestartable BuildUsingPublisherRole()
        {
            var publisher = this._publisherBuilder
                .WithExchange(this._applicationInstance.Name)
                .Build();

            var dataPublisherRole = new DataPublisherRole(publisher, new List<IConverter>());

            var startable = (IRestartable) Activator.CreateInstance(this._templateType, dataPublisherRole);
                
            var managedApplication = new ManagedApplicationRole(startable, new List<IBehaviour>());

            return managedApplication;
        }

        private IRestartable BuildUsingReceiverRole()
        {
            var relatedPublishers = _schemaDataContext.Bindings
                .Where(w => w.ReceiverId == this._applicationInstance.Id)
                .Select(s => s.PublisherApplicationInstance.Name);

            var receiver = this._receiverBuilder
                .WithQueue(this._applicationInstance.Name)
                .WithRoutingKeys(relatedPublishers)
                .Build();

            var dataReceiverRole = new DataReceiverRole(receiver, new List<IConverter>());

            var application = (IRestartable) Activator.CreateInstance(this._templateType, dataReceiverRole);
             
            var managedApplication = new ManagedApplicationRole(application, new List<IBehaviour>());

            return managedApplication;
        }
    }
}