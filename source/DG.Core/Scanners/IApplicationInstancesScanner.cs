namespace DG.Core.Scanners
{
    using System.Collections.Generic;

    public interface IApplicationInstancesScanner
    {
        IDictionary<string, string> GetInstancesTypeNames();

        IDictionary<string, string> GetFieldValuesFromApps(string fieldName);

        void Initialize();
    }
}
