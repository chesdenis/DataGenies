namespace DG.Core.Providers
{
    using System;
    using System.Collections.Generic;

    public interface IAssemblyTypesProvider
    {
        Type GetInstanceType(string typeName, string assemblyPath);
        
        IEnumerable<Type> GetTypes(string assemblyPath);
    }
}
