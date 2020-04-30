namespace DG.Core.Scanners
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Extensions.Configuration;

    public class ConfigJSONFileScanner : IConfigScanner
    {
        private readonly string configPath;
        private readonly char pathToSectionDelimiter = ':';

        public ConfigJSONFileScanner(string pathToConfig)
        {
            this.configPath = pathToConfig;
            this.InitializeRoot();
        }

        private IConfigurationRoot ConfigurationRoot { get; set; }

        public IDictionary<string, object> ConvertConfigToDictionary()
        {
            var configurationSections = this.GetFirstLevelConfigSections();
            IDictionary<string, object> sectionValues = new Dictionary<string, object>();
            foreach (ConfigurationSection configurationSection in configurationSections)
            {
                var sectionData = this.ConvertSectionToDictionary(configurationSection);
                sectionValues = sectionValues.Concat(sectionData).ToDictionary(kv => kv.Key, kv => kv.Value);
            }

            return sectionValues;
        }

        public IDictionary<string, object> ConvertSectionToDictionary(IConfigurationSection section)
        {
            var children = section.GetChildren();
            IDictionary<string, object> dictionaryInner = new Dictionary<string, object>();
            IDictionary<string, object> dictionaryUpLevel = new Dictionary<string, object>();
            if (children.Count() > 0)
            {
                foreach (var child in children)
                {
                    dictionaryInner = this.ConvertSectionToDictionary(child);
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

        public IConfigurationSection GetConfigSectionByPath(string pathToSection)
        {
            IConfigurationSection section = null;
            var pathElements = pathToSection.Split(this.pathToSectionDelimiter);
            for (int i = 0; i < pathElements.Length; i++)
            {
                if (i == 0)
                {
                    section = this.GetFirstLevelConfigSections().Where(s => s.Key == pathElements[i]).SingleOrDefault();
                }
                else
                {
                    var sections = section.GetChildren();
                    section = sections.Where(s => s.Key == pathElements[i]).SingleOrDefault();
                }
            }

            return section;
        }

        private void InitializeRoot()
        {
            if (!File.Exists(this.configPath))
            {
                throw new FileNotFoundException();
            }

            IConfigurationRoot configurationRoot =
               new ConfigurationBuilder().
               AddJsonFile(this.configPath).
               Build();
            this.ConfigurationRoot = configurationRoot;
        }

        private IEnumerable<IConfigurationSection> GetFirstLevelConfigSections()
        {
            return this.ConfigurationRoot.GetChildren();
        }
    }
}