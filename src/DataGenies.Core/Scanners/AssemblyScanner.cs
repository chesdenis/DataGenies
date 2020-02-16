using System;
using System.Collections;
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
        private readonly DataGeniesOptions _options;

        public AssemblyScanner(DataGeniesOptions options)
        {
            _options = options;
        }

        public virtual IEnumerable<Type> GetAssemblyTypes(string assemblyFullPath)
        {
            var asm = Assembly.LoadFile(assemblyFullPath);
            return asm.GetTypes()
                .Where(w => w.IsClass);
        }

        public IEnumerable<ApplicationTemplateInfo> ScanApplicationTemplates(string assemblyFullPath)
        {
            
            var types = GetAssemblyTypes(assemblyFullPath)
                .Where(w => w.GetCustomAttributes().Any(ww => ww.GetType() == typeof(ApplicationTemplateAttribute)));

            foreach (var type in types)
            {
                yield return new ApplicationTemplateInfo
                {
                    AssemblyPath = assemblyFullPath,
                    TemplateName = type.Name,
                    AssemblyVersion = GetApplicationTypeVersionFromPackagePath(assemblyFullPath)
                };
            }
        }

        public IEnumerable<BehaviourInfo> ScanBehaviours(string assemblyFullPath)
        {
            var types = GetAssemblyTypes(assemblyFullPath)
                .Where(w => w.IsClass)
                .Where(w => w.GetCustomAttributes().Any(ww => ww.GetType() == typeof(BehaviourTemplateAttribute)));

            foreach (var type in types)
            {
                yield return new BehaviourInfo()
                {
                    AssemblyPath = assemblyFullPath,
                    BehaviourName = type.Name,
                    AssemblyVersion = GetApplicationTypeVersionFromPackagePath(assemblyFullPath)
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