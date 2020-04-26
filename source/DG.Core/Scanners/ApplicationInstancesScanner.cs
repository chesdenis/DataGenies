namespace DG.Core.Scanners
{
    using System.Collections.Generic;

    public class ApplicationInstancesScanner : IApplicationInstancesScanner
    {
        private readonly IConfigScanner configScanner;
        private readonly string sectionName = "ApplicationInstances";
        private readonly IDictionary<string, object> applicationInstancesData = new Dictionary<string, object>();

        public ApplicationInstancesScanner(IConfigScanner configScanner)
        {
            this.configScanner = configScanner;
        }

        public void Initialize()
        {
            IDictionary<string, object> instancesData = this.configScanner.GetKeyValuesFromSection(this.sectionName);
            var instances = instancesData.Values;
            foreach (var instance in instances)
            {
                var instanceFields = (instance as IDictionary<string, object>).Values;
                {
                    foreach (var field_values in instanceFields)
                    {
                        IDictionary<string, object> instanceFieldsData = new Dictionary<string, object>();
                        foreach (var field_value in (IDictionary<string, object>)field_values)
                        {
                            instanceFieldsData.Add(field_value.Key, field_value.Value);
                        }

                        var instanceType = instanceFieldsData["Type"];
                        var instanceName = instanceFieldsData["Name"];
                        var newInstanceKey = instanceType + "^" + instanceName;
                        this.applicationInstancesData.Add(newInstanceKey, instanceFieldsData);
                    }
                }
            }
        }

        public IDictionary<string, string> GetInstancesTypeNames()
        {
            return this.GetFieldValuesFromApps("Type");
        }

        public IDictionary<string, string> GetFieldValuesFromApps(string fieldName)
        {
            IDictionary<string, string> fieldValues = new Dictionary<string, string>();
            foreach (var instance in this.applicationInstancesData)
            {
                var fields = instance.Value;
                var fieldValue = ((IDictionary<string, object>)fields)[fieldName];
                fieldValues.Add(instance.Key, fieldValue.ToString());
            }

            return fieldValues;
        }
    }
}