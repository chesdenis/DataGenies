using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Models;
using DataGenies.Core.Repositories;

namespace DataGenies.Core.Scanners
{
    public class BehavioursTemplatesScanner : IBehaviourTemplatesScanner
    {
        private readonly DataGeniesOptions _options;
        private readonly IFileSystemRepository _fileSystemRepository;
        private readonly IAssemblyScanner _assemblyScanner;

        public BehavioursTemplatesScanner(DataGeniesOptions options, IFileSystemRepository fileSystemRepository, IAssemblyScanner assemblyScanner)
        {
            _options = options;
            _fileSystemRepository = fileSystemRepository;
            _assemblyScanner = assemblyScanner;
        }
        
        public IEnumerable<BehaviourTemplateEntity> ScanTemplates()
        {
            return _options.DropFolderOptions.UseZippedPackages ? this.ScanInsideZippedPackages() : this.ScanAsRegularPackages();
        }
        
        public Type FindType(BehaviourTemplateEntity behaviourTemplateEntity)
        {
            var allTemplates = this.ScanTemplates();
            var matchTemplate = allTemplates.First(f => f.IsMatch(behaviourTemplateEntity));
            var templateType = Assembly.LoadFile(matchTemplate.AssemblyPath).GetType(matchTemplate.Name, true);

            return templateType;
        }
 
        private IEnumerable<BehaviourTemplateEntity> ScanInsideZippedPackages()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<BehaviourTemplateEntity> ScanAsRegularPackages()
        {
            var assemblies = _fileSystemRepository.GetFilesInFolder(_options.DropFolderOptions.Path, "*.dll");

            foreach (var assemblyPath in assemblies)
            {
                var types = this._assemblyScanner.ScanApplicationTemplates(assemblyPath);

                foreach (var appTypeInfo in types)
                {
                    yield return new BehaviourTemplateEntity
                    {
                        Name = appTypeInfo.TemplateName,
                        Version = appTypeInfo.AssemblyVersion,
                        AssemblyPath = appTypeInfo.AssemblyPath
                    };
                }
            }
        }
    }
}