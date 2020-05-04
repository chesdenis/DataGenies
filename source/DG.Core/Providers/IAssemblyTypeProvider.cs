namespace DG.Core.Providers
{
    using System;
    using System.Collections.Generic;

    public interface IAssemblyTypeProvider
    {
        IEnumerable<Type> GetTypes(string assemblyPath);
    }
}
