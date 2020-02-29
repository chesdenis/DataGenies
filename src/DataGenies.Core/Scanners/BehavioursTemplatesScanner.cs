using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataGenies.Core.Models;
using DataGenies.Core.Repositories;

namespace DataGenies.Core.Scanners
{
    public class BehavioursTemplatesScanner : IBehaviourTemplatesScanner
    {
        private readonly IAssemblyScanner _assemblyScanner;
        private readonly IFileSystemRepository _fileSystemRepository;

        public BehavioursTemplatesScanner(
            IFileSystemRepository fileSystemRepository,
            IAssemblyScanner assemblyScanner)
        {
            this._fileSystemRepository = fileSystemRepository;
            this._assemblyScanner = assemblyScanner;
        }

        public IEnumerable<BehaviourTemplateEntity> ScanTemplates(string dropFolder)
        {
            return this.ScanAsRegularPackages(dropFolder);
        }

        public Type FindType(BehaviourTemplateEntity behaviourTemplateEntity)
        {
            var allTemplates =  this._assemblyScanner.ScanBehaviourTemplates(behaviourTemplateEntity.AssemblyPath);
            var matchTemplate = allTemplates.First(f => f.IsMatch(behaviourTemplateEntity));
            var templateType = Assembly.LoadFile(matchTemplate.AssemblyPath).GetType(matchTemplate.Name, true);

            return templateType;
        }

        private IEnumerable<BehaviourTemplateEntity> ScanInsideZippedPackages()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<BehaviourTemplateEntity> ScanAsRegularPackages(string dropFolder)
        {
            var assemblies = this._fileSystemRepository.GetFilesInFolder(dropFolder, "*.dll|*.exe");

            foreach (var assemblyPath in assemblies)
            {
                var types = this._assemblyScanner.ScanBehaviourTemplates(assemblyPath);

                foreach (var appTypeInfo in types)
                {
                    yield return new BehaviourTemplateEntity
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