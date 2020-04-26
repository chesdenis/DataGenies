namespace DG.Core.Scanners
{
    using System.Collections.Generic;

    public interface IConfigScanner
    {
        IDictionary<string, object> GetKeyValuesFromSection(string sectionName);

        IDictionary<string, object> GetKeyValuesFromAllSections();

        IDictionary<string, object> GetFieldData(string sectionName, string fieldName);
    }
}
