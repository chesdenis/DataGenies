using System;
using System.Collections.Generic;
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
        
        public IEnumerable<Behaviour> ScanBehaviours()
        {
            return _options.DropFolderOptions.UseZippedPackages ? this.ScanInsideZippedPackages() : this.ScanAsRegularPackages();
        }
        
        private IEnumerable<Behaviour> ScanInsideZippedPackages()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Behaviour> ScanAsRegularPackages()
        {
            var assemblies = _fileSystemRepository.GetFilesInFolder(_options.DropFolderOptions.Path, "*.dll");

            foreach (var assemblyPath in assemblies)
            {
                var types = this._assemblyScanner.ScanApplicationTemplates(assemblyPath);

                foreach (var appTypeInfo in types)
                {
                    yield return new Behaviour
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