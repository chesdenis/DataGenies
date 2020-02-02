using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DataGenies.Core.Models;

namespace DataGenies.InMemory
{
    public class SchemaDataBuilder
    {
        private readonly SchemaDataContext _schemaDataContext;

        private ApplicationTemplate _scopeApplicationTemplate;
        private ApplicationInstance _scopeApplicationInstance;

        private string _scopeConfig = "{}";

        public SchemaDataBuilder(ISchemaDataContext schemaDataContext)
        {
            _schemaDataContext = (SchemaDataContext)schemaDataContext;
        }

        public SchemaDataBuilder CreateApplicationTemplate(string templateName, string templateVersion)
        {
            _scopeApplicationTemplate = new ApplicationTemplate
            {
                Id = this._schemaDataContext.ApplicationTemplates.Count() + 1,
                Name = templateName,
                Version = templateVersion,
                ConfigTemplateJson = _scopeConfig,
                AssemblyPath = string.Empty
            };

            _scopeConfig = "{}";

            _schemaDataContext.ApplicationTemplates.Add(_scopeApplicationTemplate);

            return this;
        }

        public SchemaDataBuilder UsingConfig(Dictionary<string, string> config)
        {
            _scopeConfig = JsonSerializer.Serialize(config);
            return this;
        }

        public SchemaDataBuilder CreateApplicationInstance(string instanceName)
        {
            _scopeApplicationInstance = new ApplicationInstance
            {
                Id = this._schemaDataContext.ApplicationInstances.Count() + 1,
                TemplateId = this._scopeApplicationTemplate.Id,
                Name = instanceName,
                ConfigJson = _scopeConfig,
                Template = _scopeApplicationTemplate
            };
            
            _scopeConfig = "{}";

            _schemaDataContext.ApplicationInstances.Add(_scopeApplicationInstance);

            return this;
        }

        public SchemaDataBuilder UsingExistingApplicationInstance(string instanceName)
        {
            _scopeApplicationInstance =
                this._schemaDataContext.ApplicationInstances.First(f => f.Name == instanceName);

            return this;
        }

        public SchemaDataBuilder UsingExistedApplicationTemplate(string templateName)
        {
            _scopeApplicationTemplate = this._schemaDataContext.ApplicationTemplates.First(f => f.Name == templateName);
            return this;
        }

        public SchemaDataBuilder ConfigureBinding(string publisherInstanceName, string receiverInstanceName)
        {
            var publisherInstance = _schemaDataContext.ApplicationInstances.First(f => f.Name == publisherInstanceName);
            var receiverInstance = _schemaDataContext.ApplicationInstances.First(f => f.Name == receiverInstanceName);

            _schemaDataContext.Bindings.Add(new Binding
            {
                ReceiverId = receiverInstance.Id,
                PublisherId = publisherInstance.Id,
                ReceiverApplicationInstance = receiverInstance,
                PublisherApplicationInstance = publisherInstance
            });

            return this;
        }

        public SchemaDataBuilder Save()
        {
            return this;
        }
    }
}