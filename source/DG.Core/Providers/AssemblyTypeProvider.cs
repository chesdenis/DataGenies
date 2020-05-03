namespace DG.Core.Providers
{
    using System;
    using DG.Core.Extensions;

    public class AssemblyTypeProvider : ITypeProvider
    {
        public Type GetInstanceType(string typeName, string assemblyPath)
        {
            return TypesExtensions.GetInstanceType(typeName, assemblyPath);
        }
    }
}
