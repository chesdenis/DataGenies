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

        private ApplicationTemplateEntity _scopedApplicationTemplateEntity;
        private ApplicationInstanceEntity _scopedApplicationInstanceEntity;
        private BehaviourEntity _scopedBehaviourEntity;
        private ConverterEntity _scopedConverterEntity;

        private string _scopeConfig = "{}";

        public SchemaDataBuilder(ISchemaDataContext schemaDataContext)
        {
            _schemaDataContext = (SchemaDataContext)schemaDataContext;
        }

        public SchemaDataBuilder CreateApplicationTemplate(string templateName, string templateVersion, int? id = null)
        {
            _scopedApplicationTemplateEntity = new ApplicationTemplateEntity
            {
                Id = id ?? this._schemaDataContext.ApplicationTemplates.Count() + 1,
                Name = templateName, 
                Version = templateVersion,
                ConfigTemplateJson = _scopeConfig,
                AssemblyPath = string.Empty
            };

            _scopeConfig = "{}";

            _schemaDataContext.ApplicationTemplates.Add(_scopedApplicationTemplateEntity);

            return this;
        }

        public SchemaDataBuilder UsingConfig(Dictionary<string, string> config)
        {
            _scopeConfig = JsonSerializer.Serialize(config);
            return this;
        }

        public SchemaDataBuilder CreateApplicationInstance(string instanceName, int? id = null)
        {
            _scopedApplicationInstanceEntity = new ApplicationInstanceEntity
            {
                Id = id ?? this._schemaDataContext.ApplicationInstances.Count() + 1,
                TemplateId = this._scopedApplicationTemplateEntity.Id,
                Name = instanceName,
                ConfigJson = _scopeConfig,
                TemplateEntity = _scopedApplicationTemplateEntity,
                Behaviours = new List<BehaviourEntity>(),
                Converters = new List<ConverterEntity>()
            };
            
            _scopeConfig = "{}";

            _schemaDataContext.ApplicationInstances.Add(_scopedApplicationInstanceEntity);

            return this;
        }

        public SchemaDataBuilder UsingExistingApplicationInstance(string instanceName)
        {
            _scopedApplicationInstanceEntity =
                this._schemaDataContext.ApplicationInstances.First(f => f.Name == instanceName);

            return this;
        }

        public SchemaDataBuilder UsingExistedApplicationTemplate(string templateName)
        {
            _scopedApplicationTemplateEntity = this._schemaDataContext.ApplicationTemplates.First(f => f.Name == templateName);
            return this;
        }

        public SchemaDataBuilder ConfigureBinding(string publisherInstanceName, string receiverInstanceName, string receiverRoutingKey)
        {
            var publisherInstance = _schemaDataContext.ApplicationInstances.First(f => f.Name == publisherInstanceName);
            var receiverInstance = _schemaDataContext.ApplicationInstances.First(f => f.Name == receiverInstanceName);

            _schemaDataContext.Bindings.Add(new BindingEntity
            {
                ReceiverId = receiverInstance.Id,
                PublisherId = publisherInstance.Id,
                ReceiverRoutingKey = receiverRoutingKey,
                ReceiverApplicationInstanceEntity = receiverInstance,
                PublisherApplicationInstanceEntity = publisherInstance
            });

            return this;
        }

        public SchemaDataBuilder RegisterBehaviour(string behaviourName, string behaviourVersion, int? id = null)
        {
            _scopedBehaviourEntity = new BehaviourEntity
            {
                Id = id ?? _schemaDataContext.Behaviours.Count() + 1,
                Name = behaviourName,
                Version = behaviourVersion,
                AssemblyPath = string.Empty,
                ApplicationInstances = new List<ApplicationInstanceEntity>()
            };

            _schemaDataContext.Behaviours.Add(_scopedBehaviourEntity);

            return this;
        }

        public SchemaDataBuilder ApplyBehaviour(string behaviourName = "")
        {
            if (!string.IsNullOrEmpty(behaviourName))
            {
                _scopedBehaviourEntity = _schemaDataContext.Behaviours.First(f => f.Name == behaviourName);
            }

            _scopedApplicationInstanceEntity.Behaviours.Add(_scopedBehaviourEntity);
            _scopedBehaviourEntity.ApplicationInstances.Add(_scopedApplicationInstanceEntity);

            return this;
        }

        public SchemaDataBuilder Save()
        {
            return this;
        }
    }
}