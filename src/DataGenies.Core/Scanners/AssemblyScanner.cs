using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DataGenies.Core.Attributes;
using DataGenies.Core.Models;

namespace DataGenies.Core.Scanners
{
    public class AssemblyScanner : IAssemblyScanner
    {
        public IEnumerable<ApplicationTemplateEntity> ScanApplicationTemplates(string assemblyFullPath)
        {
            var types = this.GetAssemblyTypes(assemblyFullPath)
                .Where(w => w.GetCustomAttributes().Any(ww => ww.GetType() == typeof(ApplicationTemplateAttribute)));

            foreach (var type in types)
            {
                yield return new ApplicationTemplateEntity
                {
                    AssemblyPath = assemblyFullPath,
                    Name = type.Name,
                    Version = this.GetApplicationTypeVersionFromPackagePath(assemblyFullPath)
                };
            }
        }

        public IEnumerable<BehaviourTemplateEntity> ScanBehaviourTemplates(string assemblyFullPath)
        {
            var types = this.GetAssemblyTypes(assemblyFullPath)
                .Where(w => w.IsClass)
                .Where(w => w.GetCustomAttributes().Any(ww => ww.GetType() == typeof(BehaviourTemplateAttribute)));

            foreach (var type in types)
            {
                yield return new BehaviourTemplateEntity
                {
                    AssemblyPath = assemblyFullPath,
                    Name = type.Name,
                    Version = this.GetApplicationTypeVersionFromPackagePath(assemblyFullPath)
                };
            }
        }

        public virtual IEnumerable<Type> GetAssemblyTypes(string assemblyFullPath)
        {
            var asm = Assembly.LoadFile(assemblyFullPath);
            return asm.GetTypes()
                .Where(w => w.IsClass);
        }

        private string GetApplicationTypeVersionFromPackagePath(string packagePath)
        {
            var packageVersion =
                packagePath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries).Last();

            return packageVersion;
        }
    }
}