using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Models;
using DataGenies.Core.Repositories;
using DataGenies.Core.Scanners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DataGenies.Core.Tests.Unit.Scanners
{
    [TestClass]
    public class ApplicationTemplatesScannerTests
    {
        [TestMethod]
        public void ShouldNotFailIfNoAnyPackage()
        {
            // Arrange
            var filesListInDropFolder = new List<string>
            {
            };
            var fileSystemRepository = Substitute.For<IFileSystemRepository>();
            fileSystemRepository.GetFilesInFolder(Arg.Any<string>(), Arg.Any<string>())
                .Returns(filesListInDropFolder);
            
            var assemblyTypesProvider = Substitute.For<IAssemblyScanner>();
            var options = new DataGeniesOptions();
            
            // Act
            var sut = new ApplicationTemplatesScanner(options, fileSystemRepository, assemblyTypesProvider);
            var data = sut.ScanTemplates().ToList();

            // Assert
            Assert.IsNotNull(data);
            Assert.IsTrue(!data.Any());
        }

        [TestMethod]
        public void ShouldScanAndGetListOfTypesInDropFolder()
        {
            // Arrange
            var filesListInDropFolder = new List<string>()
            {
                "C:\\DropFolder\\20190102.1\\CustomApplicationType1.dll",
                "C:\\DropFolder\\20190102.1\\CustomApplicationType2.dll",
                "C:\\DropFolder\\20190102.1\\CustomApplicationType2.dll"
            };
            var fileSystemRepository = Substitute.For<IFileSystemRepository>();
            fileSystemRepository.GetFilesInFolder(Arg.Any<string>(), Arg.Any<string>())
                .Returns(filesListInDropFolder);
            
            var assemblyTypesProvider = Substitute.For<IAssemblyScanner>();
            assemblyTypesProvider.ScanApplicationTemplates(Arg.Is(filesListInDropFolder[0])).Returns(
                new List<ApplicationTemplateInfo>
                {
                    new ApplicationTemplateInfo
                    {
                        AssemblyPath = filesListInDropFolder[0],
                        TemplateName = "Type1",
                        AssemblyVersion = "20190102.1"
                    },
                    new ApplicationTemplateInfo
                    {
                        AssemblyPath = filesListInDropFolder[1],
                        TemplateName = "Type2",
                        AssemblyVersion = "20190102.1"
                    }
                });
            var options = new DataGeniesOptions();
            
            // Act
            var sut = new ApplicationTemplatesScanner(options, fileSystemRepository, assemblyTypesProvider);
            var data = sut.ScanTemplates().ToList();

            // Assert
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Count() == 2);
            Assert.IsTrue(data.Any());
            Assert.AreEqual("Type1", data.First().Name);
            Assert.AreEqual(filesListInDropFolder[0], data.First().AssemblyPath);
        }
    }
}