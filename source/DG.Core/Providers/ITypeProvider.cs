namespace DG.Core.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ITypeProvider
    {
        public Type GetInstanceType(string typeName);

        public IEnumerable<Type> GetTypes();
    }
}
