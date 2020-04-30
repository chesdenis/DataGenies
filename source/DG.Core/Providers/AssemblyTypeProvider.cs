namespace DG.Core.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class AssemblyTypeProvider : ITypeProvider
    {
        private readonly string assemblyPath;

        public AssemblyTypeProvider(string assemblyPath)
        {
            this.assemblyPath = assemblyPath;
        }

        public Type GetInstanceType(string typeName)
        {
            Assembly assembly = Assembly.LoadFrom(this.assemblyPath);
            return assembly.GetType(typeName);
        }

        public IEnumerable<Type> GetTypes()
        {
            Assembly assembly = Assembly.LoadFrom(this.assemblyPath);
            return assembly.GetTypes();
        }
    }
}
