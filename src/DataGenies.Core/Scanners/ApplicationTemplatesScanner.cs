using System;
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
        private readonly DataGeniesOptions _options;

        public ApplicationTemplatesScanner(
            DataGeniesOptions options,
            IFileSystemRepository fileSystemRepository,
            IAssemblyScanner assemblyScanner)
        {
            this._options = options;
            this._fileSystemRepository = fileSystemRepository;
            this._assemblyScanner = assemblyScanner;
        }

        public IEnumerable<ApplicationTemplateEntity> ScanTemplates()
        {
            return this._options.DropFolderOptions.UseZippedPackages
                ? this.ScanTemplatesInsideZippedPackages()
                : this.ScanTemplatesAsRegularPackages();
        }

        public Type FindType(ApplicationTemplateEntity applicationTemplateEntity)
        {
            var allTemplates = this.ScanTemplates();
            var matchTemplate = allTemplates.First(f => f.IsMatch(applicationTemplateEntity));
            var templateType = Assembly.LoadFile(matchTemplate.AssemblyPath).GetType(matchTemplate.Name, true);

            return templateType;
        }

        private IEnumerable<ApplicationTemplateEntity> ScanTemplatesInsideZippedPackages()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<ApplicationTemplateEntity> ScanTemplatesAsRegularPackages()
        {
            var assemblies = this._fileSystemRepository.GetFilesInFolder(this._options.DropFolderOptions.Path, "*.dll");

            foreach (var assemblyPath in assemblies)
            {
                var types = this._assemblyScanner.ScanApplicationTemplates(assemblyPath);

                foreach (var appTypeInfo in types)
                {
                    yield return new ApplicationTemplateEntity
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