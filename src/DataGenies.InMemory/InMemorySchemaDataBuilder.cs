using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DataGenies.Core.Models;

namespace DataGenies.InMemory
{
    public class InMemorySchemaDataBuilder
    {
        private readonly InMemorySchemaDataContext _inMemorySchemaDataContext;

        private ApplicationTemplateEntity _scopedApplicationTemplateEntity;
        private ApplicationInstanceEntity _scopedApplicationInstanceEntity;
        private BehaviourEntity _scopedBehaviourEntity;
        private ConverterEntity _scopedConverterEntity;

        private string _scopeConfig = "{}";

        public InMemorySchemaDataBuilder(ISchemaDataContext schemaDataContext)
        {
            _inMemorySchemaDataContext = (InMemorySchemaDataContext)schemaDataContext;
        }

        public InMemorySchemaDataBuilder CreateApplicationTemplate(string templateName, string templateVersion, int? id = null)
        {
            _scopedApplicationTemplateEntity = new ApplicationTemplateEntity
            {
                Id = id ?? this._inMemorySchemaDataContext.ApplicationTemplates.Count() + 1,
                Name = templateName, 
                Version = templateVersion,
                ConfigTemplateJson = _scopeConfig,
                AssemblyPath = string.Empty
            };

            _scopeConfig = "{}";

            _inMemorySchemaDataContext.ApplicationTemplates.Add(_scopedApplicationTemplateEntity);

            return this;
        }

        public InMemorySchemaDataBuilder UsingConfig(Dictionary<string, string> config)
        {
            _scopeConfig = JsonSerializer.Serialize(config);
            return this;
        }

        public InMemorySchemaDataBuilder CreateApplicationInstance(string instanceName, int? id = null)
        {
            _scopedApplicationInstanceEntity = new ApplicationInstanceEntity
            {
                Id = id ?? this._inMemorySchemaDataContext.ApplicationInstances.Count() + 1,
                TemplateId = this._scopedApplicationTemplateEntity.Id,
                Name = instanceName,
                ConfigJson = _scopeConfig,
                TemplateEntity = _scopedApplicationTemplateEntity,
                Behaviours = new List<BehaviourEntity>(),
                Converters = new List<ConverterEntity>()
            };
            
            _scopeConfig = "{}";

            _inMemorySchemaDataContext.ApplicationInstances.Add(_scopedApplicationInstanceEntity);

            return this;
        }

        public InMemorySchemaDataBuilder UsingExistingApplicationInstance(string instanceName)
        {
            _scopedApplicationInstanceEntity =
                this._inMemorySchemaDataContext.ApplicationInstances.First(f => f.Name == instanceName);

            return this;
        }

        public InMemorySchemaDataBuilder UsingExistedApplicationTemplate(string templateName)
        {
            _scopedApplicationTemplateEntity = this._inMemorySchemaDataContext.ApplicationTemplates.First(f => f.Name == templateName);
            return this;
        }

        public InMemorySchemaDataBuilder ConfigureBinding(string publisherInstanceName, string receiverInstanceName, string receiverRoutingKey)
        {
            var publisherInstance = _inMemorySchemaDataContext.ApplicationInstances.First(f => f.Name == publisherInstanceName);
            var receiverInstance = _inMemorySchemaDataContext.ApplicationInstances.First(f => f.Name == receiverInstanceName);

            _inMemorySchemaDataContext.Bindings.Add(new BindingEntity
            {
                ReceiverId = receiverInstance.Id,
                PublisherId = publisherInstance.Id,
                ReceiverRoutingKey = receiverRoutingKey,
                ReceiverApplicationInstanceEntity = receiverInstance,
                PublisherApplicationInstanceEntity = publisherInstance
            });

            return this;
        }

        public InMemorySchemaDataBuilder RegisterBehaviour(string behaviourName, string behaviourVersion, int? id = null)
        {
            _scopedBehaviourEntity = new BehaviourEntity
            {
                Id = id ?? _inMemorySchemaDataContext.Behaviours.Count() + 1,
                Name = behaviourName,
                Version = behaviourVersion,
                AssemblyPath = string.Empty,
                ApplicationInstances = new List<ApplicationInstanceEntity>()
            };

            _inMemorySchemaDataContext.Behaviours.Add(_scopedBehaviourEntity);

            return this;
        }

        public InMemorySchemaDataBuilder RegisterConverter(string converterName, string converterVersion, int? id = null)
        {
            _scopedConverterEntity = new ConverterEntity()
            {
                Id = id ?? _inMemorySchemaDataContext.Behaviours.Count() + 1,
                Name = converterName,
                Version = converterVersion,
                AssemblyPath = string.Empty,
                ApplicationInstances = new List<ApplicationInstanceEntity>()
            };

            _inMemorySchemaDataContext.Converters.Add(_scopedConverterEntity);

            return this;
        }
        
        public InMemorySchemaDataBuilder ApplyConverter(string converterName = "")
        {
            if (!string.IsNullOrEmpty(converterName))
            {
                _scopedConverterEntity = _inMemorySchemaDataContext.Converters.First(f => f.Name == converterName);
            }

            _scopedApplicationInstanceEntity.Converters.Add(_scopedConverterEntity);
            _scopedConverterEntity.ApplicationInstances.Add(_scopedApplicationInstanceEntity);

            return this;
        }

        public InMemorySchemaDataBuilder ApplyBehaviour(string behaviourName = "")
        {
            if (!string.IsNullOrEmpty(behaviourName))
            {
                _scopedBehaviourEntity = _inMemorySchemaDataContext.Behaviours.First(f => f.Name == behaviourName);
            }

            _scopedApplicationInstanceEntity.Behaviours.Add(_scopedBehaviourEntity);
            _scopedBehaviourEntity.ApplicationInstances.Add(_scopedApplicationInstanceEntity);

            return this;
        }

        public InMemorySchemaDataBuilder Save()
        {
            return this;
        }
    }
}