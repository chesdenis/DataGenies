using System;
using System.Collections.Generic;
using DataGenies.AspNetCore.DataGeniesCore.Models;
using DataGenies.AspNetCore.DataGeniesCore.Providers;
using DataGenies.AspNetCore.DataGeniesCore.Repositories;

namespace DataGenies.AspNetCore.DataGeniesCore.Scanners
{
    public class ApplicationTypesScanner : IApplicationTypesScanner
    {
        private readonly DataGeniesOptions _options;
        private readonly IFileSystemRepository _fileSystemRepository;
        private readonly IAssemblyTypesProvider _assemblyTypesProvider;

        public ApplicationTypesScanner(DataGeniesOptions options, IFileSystemRepository fileSystemRepository, IAssemblyTypesProvider assemblyTypesProvider)
        {
            _options = options;
            _fileSystemRepository = fileSystemRepository;
            _assemblyTypesProvider = assemblyTypesProvider;
        }

        public IEnumerable<ApplicationType> ScanTypes()
        {
            return _options.DropFolderOptions.UseZippedPackages ? this.ScanTypesInsideZippedPackages() : this.ScanTypesAsRegularPackages();
        }

        private IEnumerable<ApplicationType> ScanTypesInsideZippedPackages()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<ApplicationType> ScanTypesAsRegularPackages()
        {
            var assemblies = _fileSystemRepository.GetFilesInFolder(_options.DropFolderOptions.Path, "*.dll");

            foreach (var assemblyPath in assemblies)
            {
                var types = this._assemblyTypesProvider.GetApplicationTypes(assemblyPath);

                foreach (var appTypeInfo in types)
                {
                    yield return new ApplicationType
                    {
                        TypeName = appTypeInfo.TypeName,
                        TypeVersion = appTypeInfo.AssemblyVersion,
                        AssemblyPath = appTypeInfo.AssemblyPath
                    };
                }
            }
        }
    }
}