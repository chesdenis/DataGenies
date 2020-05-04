namespace DG.Core.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class AssemblyTypeProvider : IAssemblyTypeProvider
    {
        public Type GetInstanceType(string typeName, string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            return assembly.GetType(typeName);
        }

        public IEnumerable<Type> GetTypes(string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            return assembly.GetTypes();
        }
    }
}
