using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Models;

namespace DataGenies.InMemory
{
    public class InMemoryConfigStore
    {
        private readonly string _contentRootPath;
        private readonly string _configStoreName;
        private readonly InMemorySchemaDataContext _schemaDataContext;

        public InMemoryConfigStore(ISchemaDataContext schemaDataContext, string contentRootPath, string configStoreName)
        {
            _contentRootPath = contentRootPath;
            _configStoreName = configStoreName;
            _schemaDataContext = (InMemorySchemaDataContext)schemaDataContext;
        }

        public void Save()
        {
            var applicationTemplates =
                _schemaDataContext.ApplicationTemplates
                    .Select(
                        s => new ApplicationTemplateEntity
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Version = s.Version,
                            AssemblyPath = s.AssemblyPath,
                            ConfigTemplateJson = s.ConfigTemplateJson
                        }).ToArray();

            var applicationInstances =
                _schemaDataContext.ApplicationInstances
                    .Select(
                        s => new ApplicationInstanceEntity
                        {
                            Id = s.Id,
                            TemplateId = s.TemplateId,
                            Name = s.Name,
                            ParametersDictAsJson = s.ParametersDictAsJson
                        }).ToArray();

            var behavioursTemplates = _schemaDataContext.BehaviourTemplates
                .Select(
                    s => new BehaviourTemplateEntity
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Version = s.Version,
                        AssemblyPath = s.AssemblyPath,
                        ConfigTemplateJson = s.ConfigTemplateJson
                    }).ToArray();

            var behavioursInstances = _schemaDataContext.BehaviourInstances
                .Select(
                    s => new BehaviourInstanceEntity
                    {
                        Id = s.Id,
                        TemplateId = s.TemplateId,
                        Name = s.Name,
                        ParametersDictAsJson = s.ParametersDictAsJson,
                        BehaviourType = s.BehaviourType,
                        BehaviourScope = s.BehaviourScope
                    }).ToArray();

            var bindings = _schemaDataContext.Bindings
                .Select(
                    s => new BindingEntity
                    {
                        ReceiverId = s.ReceiverId,
                        PublisherId = s.PublisherId,
                        ReceiverRoutingKey = s.ReceiverRoutingKey,
                    }).ToArray();

            var applicationInstancesBehaviours =
                _schemaDataContext.ApplicationInstances.Select(
                    s => s.Behaviours
                        .Select(
                            ss => new BehaviourApplicationInstanceMapping
                            {
                                BehaviourId = ss.Id, ApplicationInstanceId = s.Id
                            })).SelectMany(s => s)
                    .ToArray();

            var configStore = new ConfigStoreContent
            {
                ApplicationTemplates = applicationTemplates,
                ApplicationInstances = applicationInstances,
                BehavioursTemplates = behavioursTemplates,
                BehavioursInstances = behavioursInstances,
                Bindings = bindings,
                ApplicationInstancesBehaviours = applicationInstancesBehaviours
            };

            var configStoreAsJson = JsonSerializer.Serialize(configStore);
            
            File.WriteAllText(Path.Combine(_contentRootPath, _configStoreName), configStoreAsJson, Encoding.UTF8);
        }

        public void Read()
        {
            _schemaDataContext.Bindings.Local.Clear();
            _schemaDataContext.ApplicationInstances.Local.Clear();
            _schemaDataContext.ApplicationTemplates.Local.Clear();
            _schemaDataContext.BehaviourInstances.Local.Clear();
            _schemaDataContext.BehaviourTemplates.Local.Clear();

            var configStoreAsJson = File.ReadAllText(Path.Combine(_contentRootPath, _configStoreName), Encoding.UTF8);

            var configStoreContent = JsonSerializer.Deserialize<ConfigStoreContent>(configStoreAsJson);

            foreach (var applicationTemplateEntity in configStoreContent.ApplicationTemplates)
            {
                _schemaDataContext.ApplicationTemplates.Add(applicationTemplateEntity);
            }
        }
    }
}