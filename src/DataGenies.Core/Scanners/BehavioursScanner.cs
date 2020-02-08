using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Models;
using DataGenies.Core.Repositories;

namespace DataGenies.Core.Scanners
{
    public class BehavioursScanner : IApplicationBehavioursScanner
    {
        private readonly DataGeniesOptions _options;
        private readonly IFileSystemRepository _fileSystemRepository;
        private readonly IAssemblyScanner _assemblyScanner;

        public BehavioursScanner(DataGeniesOptions options, IFileSystemRepository fileSystemRepository, IAssemblyScanner assemblyScanner)
        {
            _options = options;
            _fileSystemRepository = fileSystemRepository;
            _assemblyScanner = assemblyScanner;
        }
        
        public IEnumerable<BehaviourEntity> ScanBehaviours()
        {
            return _options.DropFolderOptions.UseZippedPackages ? this.ScanInsideZippedPackages() : this.ScanAsRegularPackages();
        }

        public IEnumerable<IBehaviour> GetBehavioursInstances(IEnumerable<BehaviourEntity> behaviours)
        {
            var allBehaviours = this.ScanBehaviours();
            var matchedBehaviours = allBehaviours
                .Where(w => behaviours.Any(c => c.IsMatch(w)));
            return (IEnumerable<IBehaviour>)matchedBehaviours
                .Select(s => 
                    Activator.CreateInstance(
                        Assembly.LoadFile(s.AssemblyPath).GetType(s.Name, true)));
        }

        private IEnumerable<BehaviourEntity> ScanInsideZippedPackages()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<BehaviourEntity> ScanAsRegularPackages()
        {
            var assemblies = _fileSystemRepository.GetFilesInFolder(_options.DropFolderOptions.Path, "*.dll");

            foreach (var assemblyPath in assemblies)
            {
                var types = this._assemblyScanner.ScanApplicationTemplates(assemblyPath);

                foreach (var appTypeInfo in types)
                {
                    yield return new BehaviourEntity
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