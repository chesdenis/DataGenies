namespace DG.Core.Providers
{
    using System;
    using System.Collections.Generic;
    using DG.Core.Extensions;

    public class AssemblyTypeProvider : ITypeProvider
    {
        private readonly string assemblyPath;

        public AssemblyTypeProvider(string assemblyPath)
        {
            this.assemblyPath = assemblyPath;
        }

        public Type GetInstanceType(string typeName)
        {
            return TypesExtensions.GetType(this.assemblyPath, typeName);
        }

        public IEnumerable<Type> GetTypes()
        {
            return TypesExtensions.GetAssemblyTypes(this.assemblyPath);
        }
    }
}
