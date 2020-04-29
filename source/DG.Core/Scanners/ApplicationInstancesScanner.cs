namespace DG.Core.Scanners
{
    using System.Collections.Generic;
    using DG.Core.Extensions;
    using System.Linq;

    public class ApplicationInstancesScanner : IApplicationInstancesScanner
    {
        private readonly IConfigScanner configScanner;
        private readonly string sectionName = "ApplicationInstances";
        private readonly string typeConfigFieldKey = "Type";
        private readonly string nameConfigFieldKey = "Name";
        private readonly IDictionary<string, object> applicationInstancesConfiguration = new Dictionary<string, object>();

        public ApplicationInstancesScanner(IConfigScanner configScanner)
        {
            this.configScanner = configScanner;
        }

        public void Initialize()
        {
            var instancesConfigurationAsDictionary = this.configScanner.GetKeyValuesFromSection(this.sectionName);
            var instances = instancesConfigurationAsDictionary.Values;
            foreach (var instance in instances)
            {
                var instanceFields = (instance as IDictionary<string, object>).Values;
                {
                    foreach (var field_values in instanceFields)
                    {
                        var instanceFieldsData = new Dictionary<string, object>();
                        foreach (var field_value in (IDictionary<string, object>)field_values)
                        {
                            instanceFieldsData.Add(field_value.Key, field_value.Value);
                        }

                        var newInstanceKey = ApplicationExtensions
                            .ConstructUniqueId(
                            instanceFieldsData[this.typeConfigFieldKey].ToString(),
                            instanceFieldsData[this.nameConfigFieldKey].ToString());

                        this.applicationInstancesConfiguration.Add(newInstanceKey, instanceFieldsData);
                    }
                }
            }
        }

        public IDictionary<string, string> GetInstancesNamesAndTypes()
        {
            return this.GetFieldValuesFromApps(this.typeConfigFieldKey);
        }

        public IDictionary<string, string> GetFieldValuesFromApps(string fieldName)
        {
            var fieldValues = new Dictionary<string, string>();
            foreach (var instance in this.applicationInstancesConfiguration)
            {
                var fields = instance.Value;
                var fieldValue = ((IDictionary<string, object>)fields)[fieldName];
                fieldValues.Add(instance.Key, fieldValue.ToString());
            }

            return fieldValues;
        }
    }
}