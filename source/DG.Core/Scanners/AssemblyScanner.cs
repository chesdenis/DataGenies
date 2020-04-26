namespace DG.Core.Scanners
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class AssemblyScanner : IAssemblyScanner
    {
        public IEnumerable<Type> GetAssemblyTypes(string assemblyPath)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly.GetTypes();
        }

        public Type GetType(string assemblyPath, string namespaceName, string typeName)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly.GetType(namespaceName + "." + typeName);
        }
    }
}
