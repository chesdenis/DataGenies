using System.Linq;
using DataGenies.Core.Models;
using DataGenies.Core.Services;

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
        
        public void ConfigureFor(IManagedService managedService, ApplicationInstanceEntity applicationInstanceEntity)
        {
            if (managedService.GetType().IsSubclassOf(typeof(ManagedReceiverAndPublisherService))
            || 
            managedService.GetType().IsSubclassOf(typeof(ManagedReceiverAndPublisherServiceWithContainer)))
            {
                ConfigureMqForReceiverRole(applicationInstanceEntity);
                ConfigureMqForPublisherRole(applicationInstanceEntity);
            }
            else if (managedService.GetType().IsSubclassOf(typeof(ManagedReceiverService)) 
                     || 
                     managedService.GetType().IsSubclassOf(typeof(ManagedReceiverServiceWithContainer)))
            {
                ConfigureMqForReceiverRole(applicationInstanceEntity);
            }
            else if (managedService.GetType().IsSubclassOf(typeof(ManagedPublisherService))
            || 
            managedService.GetType().IsSubclassOf(typeof(ManagedPublisherServiceWithContainer))
            )
            {
                ConfigureMqForPublisherRole(applicationInstanceEntity);
            }
        }
        
        private void ConfigureMqForPublisherRole(ApplicationInstanceEntity applicationInstanceEntity)
        {
            var relatedReceivers = _schemaDataContext.Bindings
                .Where(w => w.PublisherId == applicationInstanceEntity.Id);

            this._mqConfigurator.EnsureExchange(applicationInstanceEntity.Name);

            foreach (var receiver in relatedReceivers)
            {
                this._mqConfigurator.EnsureQueue(receiver.ReceiverApplicationInstanceEntity.Name, 
                    applicationInstanceEntity.Name,
                    $"{receiver.ReceiverRoutingKey}");
            }
        }

        private void ConfigureMqForReceiverRole(ApplicationInstanceEntity applicationInstanceEntity)
        {
            var relatedPublishers = _schemaDataContext.Bindings
                .Where(w => w.ReceiverId == applicationInstanceEntity.Id);

            foreach (var publisher in relatedPublishers)
            {
                this._mqConfigurator.EnsureExchange(publisher.PublisherApplicationInstanceEntity.Name);
                this._mqConfigurator.EnsureQueue(
                    applicationInstanceEntity.Name, 
                    publisher.PublisherApplicationInstanceEntity.Name,
                    publisher.ReceiverRoutingKey);
            }
        }
    }
}