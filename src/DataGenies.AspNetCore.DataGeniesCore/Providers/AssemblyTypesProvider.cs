using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DataGenies.AspNetCore.DataGeniesCore.Attributes;
using DataGenies.AspNetCore.DataGeniesCore.Models;

namespace DataGenies.AspNetCore.DataGeniesCore.Providers
{
    public class AssemblyTypesProvider : IAssemblyTypesProvider
    {
        private readonly DataGeniesOptions _options;

        public AssemblyTypesProvider(DataGeniesOptions options)
        {
            _options = options;
        }
        
        public IEnumerable<ApplicationTypeInsideAssembly> GetApplicationTypes(string assemblyFullPath)
        {
            var asm = Assembly.LoadFile(assemblyFullPath);
            var types = asm.GetTypes()
                .Where(w => w.IsClass)
                .Where(w => w.GetCustomAttributes().Any(ww => ww.GetType() == typeof(ApplicationTypeAttribute)));

            foreach (var type in types)
            {
                yield return new ApplicationTypeInsideAssembly
                {
                    AssemblyPath = assemblyFullPath,
                    TypeName = type.Name,
                    PackageVersion = GetApplicationTypeVersionFromPackagePath(assemblyFullPath)
                };
            }
        }
        
        private string GetApplicationTypeVersionFromPackagePath(string packagePath)
        {
            var relativePath = packagePath.Replace(this._options.DropFolderOptions.Path, string.Empty);
            var packageVersion = relativePath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries)[0];

            return packageVersion;
        }
    }
}