using System.Collections.Generic;
using System.Linq;
using DataGenies.AspNetCore.DataGeniesCore.Models;
using DataGenies.AspNetCore.DataGeniesCore.Providers;
using DataGenies.AspNetCore.DataGeniesCore.Repositories;
using DataGenies.AspNetCore.DataGeniesCore.Scanners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DataGenies.AspNetCore.DataGeniesCore.Tests.Scanners
{
    [TestClass]
    public class ApplicationTypesScannerTests
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
            
            var assemblyTypesProvider = Substitute.For<IAssemblyTypesProvider>();
            var options = new DataGeniesOptions();
            
            // Act
            var sut = new ApplicationTypesScanner(options, fileSystemRepository, assemblyTypesProvider);
            var data = sut.ScanTypes().ToList();

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
            
            var assemblyTypesProvider = Substitute.For<IAssemblyTypesProvider>();
            assemblyTypesProvider.GetApplicationTypes(Arg.Is(filesListInDropFolder[0])).Returns(
                new List<ApplicationTypeInsideAssembly>
                {
                    new ApplicationTypeInsideAssembly
                    {
                        AssemblyPath = filesListInDropFolder[0],
                        TypeName = "Type1",
                        AssemblyVersion = "20190102.1"
                    },
                    new ApplicationTypeInsideAssembly
                    {
                        AssemblyPath = filesListInDropFolder[1],
                        TypeName = "Type2",
                        AssemblyVersion = "20190102.1"
                    }
                });
            var options = new DataGeniesOptions();
            
            // Act
            var sut = new ApplicationTypesScanner(options, fileSystemRepository, assemblyTypesProvider);
            var data = sut.ScanTypes().ToList();

            // Assert
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Count() == 2);
            Assert.IsTrue(data.Any());
            Assert.AreEqual("Type1", data.First().TypeName);
            Assert.AreEqual(filesListInDropFolder[0], data.First().AssemblyPath);
        }
    }
}