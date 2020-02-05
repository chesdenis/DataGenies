using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DataGenies.Core.Models;

namespace DataGenies.InMemory
{
    public class SchemaDataBuilder
    {
        private readonly SchemaDataContext _schemaDataContext;

        private ApplicationTemplate _scopedApplicationTemplate;
        private ApplicationInstance _scopedApplicationInstance;
        private Behaviour _scopedBehaviour;
        private Converter _scopedConverter;

        private string _scopeConfig = "{}";

        public SchemaDataBuilder(ISchemaDataContext schemaDataContext)
        {
            _schemaDataContext = (SchemaDataContext)schemaDataContext;
        }

        public SchemaDataBuilder CreateApplicationTemplate(string templateName, string templateVersion)
        {
            _scopedApplicationTemplate = new ApplicationTemplate
            {
                Id = this._schemaDataContext.ApplicationTemplates.Count() + 1,
                Name = templateName,
                Version = templateVersion,
                ConfigTemplateJson = _scopeConfig,
                AssemblyPath = string.Empty
            };

            _scopeConfig = "{}";

            _schemaDataContext.ApplicationTemplates.Add(_scopedApplicationTemplate);

            return this;
        }

        public SchemaDataBuilder UsingConfig(Dictionary<string, string> config)
        {
            _scopeConfig = JsonSerializer.Serialize(config);
            return this;
        }

        public SchemaDataBuilder CreateApplicationInstance(string instanceName)
        {
            _scopedApplicationInstance = new ApplicationInstance
            {
                Id = this._schemaDataContext.ApplicationInstances.Count() + 1,
                TemplateId = this._scopedApplicationTemplate.Id,
                Name = instanceName,
                ConfigJson = _scopeConfig,
                Template = _scopedApplicationTemplate
            };
            
            _scopeConfig = "{}";

            _schemaDataContext.ApplicationInstances.Add(_scopedApplicationInstance);

            return this;
        }

        public SchemaDataBuilder UsingExistingApplicationInstance(string instanceName)
        {
            _scopedApplicationInstance =
                this._schemaDataContext.ApplicationInstances.First(f => f.Name == instanceName);

            return this;
        }

        public SchemaDataBuilder UsingExistedApplicationTemplate(string templateName)
        {
            _scopedApplicationTemplate = this._schemaDataContext.ApplicationTemplates.First(f => f.Name == templateName);
            return this;
        }

        public SchemaDataBuilder ConfigureBinding(string publisherInstanceName, string receiverInstanceName, string receiverRoutingKey)
        {
            var publisherInstance = _schemaDataContext.ApplicationInstances.First(f => f.Name == publisherInstanceName);
            var receiverInstance = _schemaDataContext.ApplicationInstances.First(f => f.Name == receiverInstanceName);

            _schemaDataContext.Bindings.Add(new Binding
            {
                ReceiverId = receiverInstance.Id,
                PublisherId = publisherInstance.Id,
                ReceiverRoutingKey = receiverRoutingKey,
                ReceiverApplicationInstance = receiverInstance,
                PublisherApplicationInstance = publisherInstance
            });

            return this;
        }

        public SchemaDataBuilder RegisterBehaviour(string behaviourName, string behaviourVersion)
        {
            _scopedBehaviour = new Behaviour
            {
                Id = _schemaDataContext.Behaviours.Count() + 1,
                Name = behaviourName,
                Version = behaviourVersion,
                AssemblyPath = string.Empty,
                ApplicationInstances = new List<ApplicationInstance>()
            };
            
            _schemaDataContext.Behaviours.Add(_scopedBehaviour);

            return this;
        }

        public SchemaDataBuilder ApplyBehaviour(string behaviourName)
        {
            throw new NotImplementedException();
        }

        public SchemaDataBuilder Save()
        {
            return this;
        }
    }
}