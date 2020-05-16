using System;
using System.Collections.Generic;
using System.Linq;
using DG.Core.Attributes;
using DG.Core.Extensions;
using DG.Core.Model.ClusterConfig;
using DG.Core.Providers;

namespace DG.Core.Scanners
{
    public class ApplicationTypesScanner : IApplicationTypesScanner
    {
        private readonly IClusterConfigProvider clusterConfigProvider;
        private readonly IFileSystemProvider fileSystemProvider;
        private readonly IAssemblyTypesProvider _assemblyTypesProvider;

        public ApplicationTypesScanner(
            IClusterConfigProvider clusterConfigProvider, 
            IFileSystemProvider fileSystemProvider, 
            IAssemblyTypesProvider assemblyTypesProvider)
        {
            this.clusterConfigProvider = clusterConfigProvider;
            this.fileSystemProvider = fileSystemProvider;
            this._assemblyTypesProvider = assemblyTypesProvider;
        }
        
        public IEnumerable<Type> Scan()
        {
            var typesSources = this.clusterConfigProvider.GetApplicationTypesSources();

            return typesSources
                .Where(w => w.PathType == "Folder")
                .SelectMany(this.ScanApplicationsInFolder)
                .Union(typesSources
                    .Where(w => w.PathType == "DirectFile")
                    .SelectMany(this.ScanApplicationsInDirectFile));
        }

        private IEnumerable<Type> ScanApplicationsInFolder(ApplicationTypeSource applicationTypeSource)
        {
            var assembliesInFolder = this.fileSystemProvider.GetAssembliesLocations(applicationTypeSource.Path);

            return assembliesInFolder.SelectMany(s =>
                this._assemblyTypesProvider.GetTypes(s)
                    .Where(w => w.HasClassAttribute(typeof(ApplicationAttribute))));
        }

        private IEnumerable<Type> ScanApplicationsInDirectFile(ApplicationTypeSource applicationTypeSource)
        {
            return this._assemblyTypesProvider.GetTypes(applicationTypeSource.Path)
                .Where(w => w.HasClassAttribute(typeof(ApplicationAttribute)));
        }
    }
}