using System;
using System.Collections.Generic;
using DataGenies.Core.Models;
using DataGenies.Core.Repositories;

namespace DataGenies.Core.Scanners
{
    public class ApplicationTemplatesScanner : IApplicationTemplatesScanner
    {
        private readonly DataGeniesOptions _options;
        private readonly IFileSystemRepository _fileSystemRepository;
        private readonly IAssemblyScanner _assemblyScanner;

        public ApplicationTemplatesScanner(DataGeniesOptions options, IFileSystemRepository fileSystemRepository, IAssemblyScanner assemblyScanner)
        {
            _options = options;
            _fileSystemRepository = fileSystemRepository;
            _assemblyScanner = assemblyScanner;
        }

        public IEnumerable<ApplicationTemplate> ScanTemplates()
        {
            return _options.DropFolderOptions.UseZippedPackages ? this.ScanTemplatesInsideZippedPackages() : this.ScanTemplatesAsRegularPackages();
        }

        private IEnumerable<ApplicationTemplate> ScanTemplatesInsideZippedPackages()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<ApplicationTemplate> ScanTemplatesAsRegularPackages()
        {
            var assemblies = _fileSystemRepository.GetFilesInFolder(_options.DropFolderOptions.Path, "*.dll");

            foreach (var assemblyPath in assemblies)
            {
                var types = this._assemblyScanner.ScanApplicationTemplates(assemblyPath);

                foreach (var appTypeInfo in types)
                {
                    yield return new ApplicationTemplate
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