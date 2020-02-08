using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataGenies.Core.Converters;
using DataGenies.Core.Models;
using DataGenies.Core.Repositories;

namespace DataGenies.Core.Scanners
{
    public class ConvertersScanner : IApplicationConvertersScanner
    {
        private readonly DataGeniesOptions _options;
        private readonly IFileSystemRepository _fileSystemRepository;
        private readonly IAssemblyScanner _assemblyScanner;

        public ConvertersScanner(DataGeniesOptions options, IFileSystemRepository fileSystemRepository,
            IAssemblyScanner assemblyScanner)
        {
            _options = options;
            _fileSystemRepository = fileSystemRepository;
            _assemblyScanner = assemblyScanner;
        }
        
        public IEnumerable<ConverterEntity> ScanConverters()
        {
            return _options.DropFolderOptions.UseZippedPackages ? this.ScanInsideZippedPackages() : this.ScanAsRegularPackages();
        }

        public IEnumerable<IConverter> GetConvertersInstances(IEnumerable<ConverterEntity> converters)
        {
            var allConverters = this.ScanConverters();
            var matchedConverters = allConverters
                .Where(w => converters.Any(c => c.IsMatch(w)));
            return (IEnumerable<IConverter>)matchedConverters
                .Select(s => 
                    Activator.CreateInstance(
                        Assembly.LoadFile(s.AssemblyPath).GetType(s.Name, true)));
        }

        private IEnumerable<ConverterEntity> ScanInsideZippedPackages()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<ConverterEntity> ScanAsRegularPackages()
        {
            var assemblies = _fileSystemRepository.GetFilesInFolder(_options.DropFolderOptions.Path, "*.dll");

            foreach (var assemblyPath in assemblies)
            {
                var types = this._assemblyScanner.ScanConverters(assemblyPath);

                foreach (var appTypeInfo in types)
                {
                    yield return new ConverterEntity
                    {
                        Name = appTypeInfo.ConverterName,
                        Version = appTypeInfo.AssemblyVersion,
                        AssemblyPath = appTypeInfo.AssemblyPath
                    };
                }
            }
        }
    }
}