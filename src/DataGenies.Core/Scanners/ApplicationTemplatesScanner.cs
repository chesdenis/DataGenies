using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataGenies.Core.Models;
using DataGenies.Core.Repositories;

namespace DataGenies.Core.Scanners
{
    public class ApplicationTemplatesScanner : IApplicationTemplatesScanner
    {
        private readonly IAssemblyScanner _assemblyScanner;
        private readonly IFileSystemRepository _fileSystemRepository;
      

        public ApplicationTemplatesScanner(
            IFileSystemRepository fileSystemRepository,
            IAssemblyScanner assemblyScanner)
        {
            this._fileSystemRepository = fileSystemRepository;
            this._assemblyScanner = assemblyScanner;
        }

        public IEnumerable<ApplicationTemplateEntity> ScanTemplates(string dropFolder)
        {
            return this.ScanAsRegularPackages(dropFolder);
        }

        public Type FindType(ApplicationTemplateEntity applicationTemplateEntity)
        {
            var applicationTemplates = this._assemblyScanner.ScanApplicationTemplates(applicationTemplateEntity.AssemblyPath);
            var matchTemplate = applicationTemplates.First(f => f.IsMatch(applicationTemplateEntity));
            var templateType = Assembly.LoadFile(matchTemplate.AssemblyPath).GetType(matchTemplate.Name, true);

            return templateType;
        }

        private IEnumerable<ApplicationTemplateEntity> ScanInsideZippedPackages(string dropFolder)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<ApplicationTemplateEntity> ScanAsRegularPackages(string dropFolder)
        {
            var assemblies = this._fileSystemRepository.GetFilesInFolder(dropFolder, "*.dll|*.exe");

            foreach (var assemblyPath in assemblies)
            {
                var types = this._assemblyScanner.ScanApplicationTemplates(assemblyPath);

                foreach (var appTypeInfo in types)
                {
                    yield return new ApplicationTemplateEntity
                    {
                        Name = appTypeInfo.Name,
                        Version = appTypeInfo.Version,
                        AssemblyPath = appTypeInfo.AssemblyPath
                    };
                }
            }
        }
    }
}