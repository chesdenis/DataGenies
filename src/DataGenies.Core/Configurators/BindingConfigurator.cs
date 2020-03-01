using System.Linq;
using DataGenies.Core.Models;

namespace DataGenies.Core.Configurators
{
    public class BindingConfigurator : IBindingConfigurator
    {
        private readonly IMqConfigurator mqConfigurator;
        private readonly IFlowSchemaContext _flowSchemaContext;

        public BindingConfigurator(
            IFlowSchemaContext flowSchemaContext,
            IMqConfigurator mqConfigurator)
        {
            this._flowSchemaContext = flowSchemaContext;
            this.mqConfigurator = mqConfigurator;
        }

        public BindingNetwork ConfigureBindingNetworkFor(int applicationInstanceEntityId)
        {
            var publishersBindingEntities = this._flowSchemaContext.Bindings
                .Where(w => w.ReceiverId == applicationInstanceEntityId);

            var receiversBindingEntities = this._flowSchemaContext.Bindings
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
                    CurrentInstanceName = s.ReceiverApplicationInstanceEntity.Name,
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
                        CurrentInstanceName = s.PublisherApplicationInstanceEntity.Name,
                    }));

            var connectedPublishers = new ConnectedPublishers();
            connectedPublishers.AddRange(
                bindingReferences
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
                Receivers = connectedReceivers,
            };
        }

        public void ConfigureBindings(BindingNetwork bindingNetwork)
        {
            foreach (var bindingReference in bindingNetwork.Publishers)
            {
                this.ConfigureFor(bindingReference);
            }

            foreach (var bindingReference in bindingNetwork.Receivers)
            {
                this.ConfigureFor(bindingReference);
            }
        }

        private void ConfigureFor(BindingReference bindingReference)
        {
            this.mqConfigurator.EnsureExchange(bindingReference.ExchangeName);
            this.mqConfigurator.EnsureQueue(
                bindingReference.QueueName,
                bindingReference.ExchangeName,
                bindingReference.RoutingKey);
        }
    }
}