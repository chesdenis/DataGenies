namespace DG.Core.Providers
{
    using System;

    public interface ITypeProvider
    {
        public Type GetInstanceType(string typeName, string assemblyPath);

    }
}
