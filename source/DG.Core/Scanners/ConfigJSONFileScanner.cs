namespace DG.Core.Scanners
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Extensions.Configuration;

    public class ConfigJSONFileScanner : IConfigScanner
    {
        private readonly string configPath;

        public ConfigJSONFileScanner(string pathToConfig)
        {
            this.configPath = pathToConfig;
        }

        public IDictionary<string, object> GetKeyValuesFromAllSections()
        {
            var configurationSections = this.GetBaseConfigSections();
            IDictionary<string, object> sectionValues = new Dictionary<string, object>();
            foreach (ConfigurationSection configurationSection in configurationSections)
            {
                var sectionData = this.ScanSection(configurationSection);
                sectionValues = sectionValues.Concat(sectionData).ToDictionary(kv => kv.Key, kv => kv.Value);
            }

            return sectionValues;
        }

        public IDictionary<string, object> GetFieldData(string sectionName, string fieldName)
        {
            IDictionary<string, object> fieldsValues = new Dictionary<string, object>();
            var sectionDataAsDictionary = this.GetKeyValuesFromSection(sectionName);
            foreach (var data in sectionDataAsDictionary)
            {
                var fields = data.Value;
                var fieldsAsDictionary = fields as IDictionary<string, object>;
                if (fieldsAsDictionary != null)
                {
                    var fieldValue = fieldsAsDictionary[fieldName];
                    fieldsValues.Add(data.Key, fieldValue);
                }
            }

            return fieldsValues;
        }

        public IDictionary<string, object> GetKeyValuesFromSection(string sectionName)
        {
            var configurationSections = this.GetBaseConfigSections();
            IDictionary<string, object> sectionValues = new Dictionary<string, object>();
            foreach (ConfigurationSection configurationSection in configurationSections)
            {
                if (configurationSection.Key == sectionName)
                {
                    sectionValues = this.ScanSection(configurationSection);
                }
            }

            return sectionValues;
        }

        private IDictionary<string, object> ScanSection(IConfigurationSection section)
        {
            var children = section.GetChildren();
            IDictionary<string, object> dictionaryInner = new Dictionary<string, object>();
            IDictionary<string, object> dictionaryUpLevel = new Dictionary<string, object>();
            if (children.Count() > 0)
            {
                foreach (var child in children)
                {
                    dictionaryInner = this.ScanSection(child);
                    if (dictionaryUpLevel.ContainsKey(section.Key))
                    {
                        IDictionary<string, object> value = (IDictionary<string, object>)dictionaryUpLevel[section.Key];
                        foreach (var kv in dictionaryInner)
                        {
                            value.Add(kv.Key, kv.Value);
                        }
                    }
                    else
                    {
                        dictionaryUpLevel.Add(section.Key, dictionaryInner);
                    }
                }
            }
            else
            {
                dictionaryInner.Add(section.Key, section.Value);
                return dictionaryInner;
            }

            return dictionaryUpLevel;
        }

        private IEnumerable<IConfigurationSection> GetBaseConfigSections()
        {
            if (!File.Exists(this.configPath))
            {
                throw new System.IO.FileNotFoundException();
            }

            IConfigurationRoot configurationRoot =
               new ConfigurationBuilder().
               AddJsonFile(this.configPath).
               Build();
            return configurationRoot.GetChildren();
        }
    }
}