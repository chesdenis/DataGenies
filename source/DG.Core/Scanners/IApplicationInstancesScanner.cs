namespace DG.Core.Scanners
{
    using System.Collections.Generic;

    public interface IApplicationInstancesScanner
    {
        IDictionary<string, string> GetInstancesNamesAndTypes();

        IDictionary<string, string> GetFieldValuesFromApps(string fieldName);

        void Initialize();
    }
}
