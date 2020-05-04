﻿using System;
using System.Collections.Generic;
using System.Linq;
using DG.Core.Model.ClusterConfig;
using DG.Core.Providers;

namespace DG.Core.Scanners
{
    public class ApplicationScanner : IApplicationScanner
    {
        private readonly IClusterConfigProvider clusterConfigProvider;
        private readonly IFileSystemProvider fileSystemProvider;
        private readonly IAssemblyTypeProvider assemblyTypeProvider;

        public ApplicationScanner(
            IClusterConfigProvider clusterConfigProvider, 
            IFileSystemProvider fileSystemProvider, 
            IAssemblyTypeProvider assemblyTypeProvider)
        {
            this.clusterConfigProvider = clusterConfigProvider;
            this.fileSystemProvider = fileSystemProvider;
            this.assemblyTypeProvider = assemblyTypeProvider;
        }
        
        public IEnumerable<Type> Scan()
        {
            var typesSources = this.clusterConfigProvider.GetApplicationTypesSources();

            return typesSources
                .Where(w => w.PathType == PathType.Folder)
                .SelectMany(this.ScanApplicationsInFolder)
                .Union(typesSources
                    .Where(w => w.PathType == PathType.DirectFile)
                    .SelectMany(this.ScanApplicationsInDirectFile));
        }

        private IEnumerable<Type> ScanApplicationsInFolder(ApplicationTypeSource applicationTypeSource)
        {
            var assembliesInFolder = this.fileSystemProvider.GetAssembliesLocations(applicationTypeSource.Path);

            return assembliesInFolder.SelectMany(s => this.assemblyTypeProvider.GetTypes(s));
        }

        private IEnumerable<Type> ScanApplicationsInDirectFile(ApplicationTypeSource applicationTypeSource)
        {
            return this.assemblyTypeProvider.GetTypes(applicationTypeSource.Path)
                .Where(f => f.Name == applicationTypeSource.Name);
        }
    }
}