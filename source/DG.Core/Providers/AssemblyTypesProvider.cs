using System.IO;

namespace DG.Core.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class AssemblyTypesProvider : IAssemblyTypesProvider
    {
        public Type GetInstanceType(string typeName, string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            return assembly.GetType(typeName);
        }

        public IEnumerable<Type> GetTypes(string assemblyPath)
        {
            try
            {
                if (File.Exists(assemblyPath))
                {
                    var assembly = Assembly.LoadFrom(assemblyPath);
                    return assembly.GetTypes();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            return new List<Type>();
        }
    }
}
