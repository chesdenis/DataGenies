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
        
        public void ConfigureFor(int instanceId)
        {
            ConfigureMqForReceiverRole(instanceId);
            ConfigureMqForPublisherRole(instanceId);
        }

        public void ConfigureForTemplateScope(int instanceId, string boundTemplateName, string routingKey = "#")
        {
            var boundInstanceNames =
                _schemaDataContext.ApplicationTemplates
                    .First(f => f.Name == boundTemplateName)
                    .ApplicationInstances.Select(s => s.Name).ToArray(); 
            
            var instanceName = _schemaDataContext.ApplicationInstances.First(f => f.Id == instanceId).Name;

            foreach (var boundInstanceName in boundInstanceNames)
            {
                ConfigureFor(instanceName, boundInstanceName, routingKey);
                ConfigureFor(boundInstanceName, instanceName, routingKey);
            }
        }

        public void ConfigureForInstanceScope(int instanceId, string boundInstanceName, string routingKey = "#")
        {
            var instanceName = _schemaDataContext.ApplicationInstances.First(f => f.Id == instanceId).Name;
            
            ConfigureFor(instanceName, boundInstanceName, routingKey);
            ConfigureFor(boundInstanceName, instanceName, routingKey);
        }
        
        private void ConfigureMqForPublisherRole(int instanceId)
        {
            var instanceName = _schemaDataContext.ApplicationInstances.First(f => f.Id == instanceId).Name;
            
            var relatedReceivers = _schemaDataContext.Bindings
                .Where(w => w.PublisherId == instanceId);

            this._mqConfigurator.EnsureExchange(instanceName);

            foreach (var receiver in relatedReceivers)
            {
                this._mqConfigurator.EnsureQueue(receiver.ReceiverApplicationInstanceEntity.Name, 
                    instanceName,
                    $"{receiver.ReceiverRoutingKey}");
            }
        }

        private void ConfigureMqForReceiverRole(int instanceId)
        {
            var instanceName = _schemaDataContext.ApplicationInstances.First(f => f.Id == instanceId).Name;

            var relatedPublishers = _schemaDataContext.Bindings
                .Where(w => w.ReceiverId == instanceId);

            foreach (var publisher in relatedPublishers)
            {
                this._mqConfigurator.EnsureExchange(publisher.PublisherApplicationInstanceEntity.Name);
                this._mqConfigurator.EnsureQueue(
                    instanceName, 
                    publisher.PublisherApplicationInstanceEntity.Name,
                    publisher.ReceiverRoutingKey);
            }
        }
        
        private void ConfigureFor(string publisherName, string receiverName, string routingKey = "#")
        {
            this._mqConfigurator.EnsureExchange(publisherName);
            this._mqConfigurator.EnsureQueue(receiverName,publisherName, $"{routingKey}");
        }
    }
}