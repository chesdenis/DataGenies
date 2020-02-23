using System;
using System.Linq;
using DataGenies.Core.Models;

namespace DataGenies.Core.Configurators
{
    public class BindingConfigurator : IBindingConfigurator
    {
        private readonly ISchemaDataContext _schemaDataContext;
        private readonly IMqConfigurator _mqConfigurator;

        public BindingConfigurator(
            ISchemaDataContext schemaDataContext,
            IMqConfigurator mqConfigurator)
        {
            _schemaDataContext = schemaDataContext;
            _mqConfigurator = mqConfigurator;
        }
        
        private void ConfigureFor(BindingReference bindingReference)
        {
            this._mqConfigurator.EnsureExchange(bindingReference.ExchangeName);
            this._mqConfigurator.EnsureQueue(bindingReference.QueueName, bindingReference.ExchangeName, bindingReference.RoutingKey);
        }

        public BindingNetwork ConfigureBindingNetworkFor(int applicationInstanceEntityId)
        {
            var publishersBindingEntities = _schemaDataContext.Bindings
                .Where(w => w.ReceiverId == applicationInstanceEntityId);

            var receiversBindingEntities = _schemaDataContext.Bindings
                .Where(w => w.PublisherId == applicationInstanceEntityId);

            var bindingReferences = publishersBindingEntities.Select(
                s => new BindingReference
                {
                    PublisherId = s.PublisherId,
                    ReceiverId = s.ReceiverId,
                    PublisherInstanceName = s.PublisherApplicationInstanceEntity.Name,
                    ReceiverInstanceName = s.ReceiverApplicationInstanceEntity.Name,
                    PublisherTemplateName = s.PublisherApplicationInstanceEntity.TemplateEntity.Name,
                    ReceiverTemplateName = s.ReceiverApplicationInstanceEntity.TemplateEntity.Name,
                    RoutingKey = s.ReceiverRoutingKey,
                    CurrentInstanceName = s.ReceiverApplicationInstanceEntity.Name
                }).Union(
                receiversBindingEntities.Select(
                    s => new BindingReference
                    {
                        PublisherId = s.PublisherId,
                        ReceiverId = s.ReceiverId,
                        PublisherInstanceName = s.PublisherApplicationInstanceEntity.Name,
                        ReceiverInstanceName = s.ReceiverApplicationInstanceEntity.Name,
                        PublisherTemplateName = s.PublisherApplicationInstanceEntity.TemplateEntity.Name,
                        ReceiverTemplateName = s.ReceiverApplicationInstanceEntity.TemplateEntity.Name,
                        RoutingKey = s.ReceiverRoutingKey,
                        CurrentInstanceName = s.PublisherApplicationInstanceEntity.Name
                    }));
            
            var connectedPublishers = new ConnectedPublishers();
            connectedPublishers.AddRange(bindingReferences
                     .Where(w => w.ReceiverId == applicationInstanceEntityId)
                     .ToList());
            
            var connectedReceivers = new ConnectedReceivers();
            connectedReceivers.AddRange(
                bindingReferences
                    .Where(w => w.PublisherId == applicationInstanceEntityId)
                    .ToList());

            return new BindingNetwork
            {
                ApplicationInstanceEntityId = applicationInstanceEntityId,
                Publishers = connectedPublishers,
                Receivers = connectedReceivers
            };
        }

        public void ConfigureBindings(BindingNetwork bindingNetwork)
        {
            foreach (var bindingReference in bindingNetwork.Publishers)
            {
                ConfigureFor(bindingReference);
            }

            foreach (var bindingReference in bindingNetwork.Receivers)
            {
                ConfigureFor(bindingReference);
            }
        }
    }
}