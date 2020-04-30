namespace DG.Core.Scanners
{
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;

    public interface IConfigScanner
    {
        IDictionary<string, object> ConvertConfigToDictionary();

        IConfigurationSection GetConfigSectionByPath(string pathToSection);

        IDictionary<string, object> ConvertSectionToDictionary(IConfigurationSection section);
    }
}
