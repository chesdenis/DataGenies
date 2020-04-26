namespace DG.Core.Scanners
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IAssemblyScanner
    {
        IEnumerable<Type> GetAssemblyTypes(string assemblyPath);

        Type GetType(string assemblyPath, string namespaceName, string typeName);
    }
}
