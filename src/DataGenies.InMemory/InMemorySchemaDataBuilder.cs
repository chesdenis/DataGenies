using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.Json;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Models;

namespace DataGenies.InMemory
{
    public class InMemorySchemaDataBuilder
    {
        private readonly InMemorySchemaDataContext _inMemorySchemaDataContext;

        private ApplicationTemplateEntity _scopedApplicationTemplateEntity;
        private ApplicationInstanceEntity _scopedApplicationInstanceEntity;
        private BehaviourTemplateEntity _scopedBehaviourTemplateEntity;
        private BehaviourInstanceEntity _scopedBehaviourInstanceEntity;
       
        private string _parametersDictAsJson = "{}";

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
                ConfigTemplateJson = this._parametersDictAsJson,
                AssemblyPath = string.Empty
            };

            this._parametersDictAsJson = "{}";

            _inMemorySchemaDataContext.ApplicationTemplates.Add(_scopedApplicationTemplateEntity);

            return this;
        }
        
        public InMemorySchemaDataBuilder CreateBehaviourTemplate(string behaviourName, string behaviourVersion, int? id = null)
        {
            this._scopedBehaviourTemplateEntity = new BehaviourTemplateEntity
            {
                Id = id ?? _inMemorySchemaDataContext.BehaviourTemplates.Count() + 1,
                Name = behaviourName,
                Version = behaviourVersion,
                AssemblyPath = string.Empty
            };

            _inMemorySchemaDataContext.BehaviourTemplates.Add(_scopedBehaviourTemplateEntity);

            return this;
        }

        public InMemorySchemaDataBuilder UsingParametersDictAsJson(Dictionary<string, string> config)
        {
            this._parametersDictAsJson = JsonSerializer.Serialize(config);
            return this;
        }

        public InMemorySchemaDataBuilder CreateApplicationInstance(string instanceName, int? id = null)
        {
            _scopedApplicationInstanceEntity = new ApplicationInstanceEntity
            {
                Id = id ?? this._inMemorySchemaDataContext.ApplicationInstances.Count() + 1,
                TemplateId = this._scopedApplicationTemplateEntity.Id,
                Name = instanceName,
                ParametersDictAsJson = this._parametersDictAsJson,
                TemplateEntity = _scopedApplicationTemplateEntity,
                Behaviours = new List<BehaviourInstanceEntity>()
            };
            
            this._parametersDictAsJson = "{}";

            _inMemorySchemaDataContext.ApplicationInstances.Add(_scopedApplicationInstanceEntity);

            return this;
        }
        
        public InMemorySchemaDataBuilder CreateBehaviourInstance(string instanceName, BehaviourType behaviourType, BehaviourScope behaviourScope, int? id = null)
        {
            this._scopedBehaviourInstanceEntity = new BehaviourInstanceEntity
            {
                Id = id ?? this._inMemorySchemaDataContext.ApplicationInstances.Count() + 1,
                TemplateId = this._scopedBehaviourTemplateEntity.Id,
                Name = instanceName,
                ParametersDictAsJson = this._parametersDictAsJson,
                BehaviourType = behaviourType,
                BehaviourScope = behaviourScope,
                TemplateEntity = this._scopedBehaviourTemplateEntity,
                ApplicationInstances = new List<ApplicationInstanceEntity>()
            };
            
            this._parametersDictAsJson = "{}";
            
            _inMemorySchemaDataContext.BehaviourInstances.Add(_scopedBehaviourInstanceEntity);
             
            return this;
        }
        
        public InMemorySchemaDataBuilder AssignBehaviour(string behaviourInstance = "", string applicationInstance = "")
        {
            var behaviourInstanceEntity = this._scopedBehaviourInstanceEntity;
            var applicationInstanceEntity = this._scopedApplicationInstanceEntity;
            
            if (!string.IsNullOrEmpty(behaviourInstance))
            {
                behaviourInstanceEntity =
                    this._inMemorySchemaDataContext.BehaviourInstances.First(f => f.Name == behaviourInstance);
            }

            if (!string.IsNullOrEmpty(applicationInstance))
            {
                applicationInstanceEntity =
                    this._inMemorySchemaDataContext.ApplicationInstances.First(f => f.Name == applicationInstance);
            }

            applicationInstanceEntity.Behaviours.Add(behaviourInstanceEntity);
            behaviourInstanceEntity.ApplicationInstances.Add(applicationInstanceEntity);

            return this;
        }


        public InMemorySchemaDataBuilder UsingExistingApplicationInstance(string instanceName)
        {
            _scopedApplicationInstanceEntity =
                this._inMemorySchemaDataContext.ApplicationInstances.First(f => f.Name == instanceName);

            return this;
        }
        
        public InMemorySchemaDataBuilder UsingExistingBehaviourInstance(string instanceName)
        {
            this._scopedBehaviourInstanceEntity =
                this._inMemorySchemaDataContext.BehaviourInstances.First(f => f.Name == instanceName);

            return this;
        }

        public InMemorySchemaDataBuilder UsingExistedApplicationTemplate(string templateName)
        {
            _scopedApplicationTemplateEntity = this._inMemorySchemaDataContext.ApplicationTemplates.First(f => f.Name == templateName);
            return this;
        }
        
        public InMemorySchemaDataBuilder UsingExistedBehaviourTemplate(string templateName)
        {
            this._scopedBehaviourTemplateEntity = this._inMemorySchemaDataContext.BehaviourTemplates.First(f => f.Name == templateName);
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
 
        public InMemorySchemaDataBuilder Save()
        {
            return this;
        }
    }
}