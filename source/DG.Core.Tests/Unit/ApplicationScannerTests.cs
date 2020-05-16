using System;
using System.Collections.Generic;
using System.Linq;
using DG.Core.Attributes;
using DG.Core.Model.ClusterConfig;
using DG.Core.Model.Enums;
using DG.Core.Model.Output;
using DG.Core.Providers;
using DG.Core.Scanners;
using FluentAssertions;
using Moq;
using Xunit;

namespace DG.Core.Tests.Unit
{
    public class ApplicationScannerTests
    {
        [Fact]
        public void ShouldScanApplicationsWithApplicationAttributeOnly()
        {
            // Arrange
            var applicationScanner = this.BuildApplicationScanner();

            // Act
            var scanResult = applicationScanner.Scan().ToList();

            // Assert
            scanResult.Should().HaveCount(4);
            scanResult.Should().ContainSingle(x => x.FullName == typeof(AppA).FullName);
            scanResult.Should().ContainSingle(x => x.FullName == typeof(AppB).FullName);
            scanResult.Should().ContainSingle(x => x.FullName == typeof(AppC).FullName);
            scanResult.Should().ContainSingle(x => x.FullName == typeof(AppE).FullName);
        }

        [Fact]
        public void ShouldIgnoreTypesWhichDontHaveApplicationAttribute()
        {
            // Arrange
            var applicationScanner = this.BuildApplicationScanner();

            // Act
            var scanResult = applicationScanner.Scan().ToList();

            // Assert
            scanResult.Should().NotContain(x => x.FullName == typeof(AppD).FullName);
        }

        [Fact]
        public void ShouldReturnEmptyIfNoAnySources()
        {
            // Arrange
            var applicationScanner = this.BuildApplicationScanner(new ApplicationTypesSources());

            // Act
            var scanResult = applicationScanner.Scan().ToList();

            // Assert
            scanResult.Should().HaveCount(0);
        }

        private ApplicationTypesScanner BuildApplicationScanner(ApplicationTypesSources customSources = null)
        {
            var clusterConfigProvider = new Mock<IClusterConfigProvider>();
            var fileSystemProvider = new Mock<IFileSystemProvider>();
            var assemblyProvider = new Mock<IAssemblyTypesProvider>();

            fileSystemProvider.Setup(x => x.GetAssembliesLocations("pathWithMultipleAssemblies")).Returns(new[]
            {
                "rootPath/assemblyA",
                "rootPath/assemblyB",
            });

            assemblyProvider
                .Setup(x =>
                    x.GetTypes("rootPath/assemblyA"))
                .Returns(new List<Type>()
                {
                    typeof(AppA),
                    typeof(AppB),
                });

            assemblyProvider
                .Setup(x =>
                    x.GetTypes("rootPath/assemblyB"))
                .Returns(new List<Type>()
                {
                    typeof(AppC),
                    typeof(AppD),
                });

            assemblyProvider
                .Setup(x =>
                    x.GetTypes("pathToDirectAssemblyC"))
                .Returns(new List<Type>()
                {
                    typeof(AppE),
                });
            
            clusterConfigProvider.Setup(x => x.GetApplicationTypesSources()).Returns(
                customSources ?? new ApplicationTypesSources()
                {
                    new ApplicationTypeSource()
                    {
                        Name = "SourceA",
                        Path = "pathWithMultipleAssemblies",
                        PathType = "Folder",
                    },
                    new ApplicationTypeSource()
                    {
                        Name = "SourceB",
                        Path = "pathToDirectAssemblyC",
                        PathType = "DirectFile",
                    },
                });

            return new ApplicationTypesScanner(
                clusterConfigProvider.Object,
                fileSystemProvider.Object,
                assemblyProvider.Object);
        }

        [Application]
        internal class AppA
        {
            [StateReport]
            public StateReport ReportStateSampleFunction()
            {
                return new StateReport()
                {
                    Status = Status.Finished,
                };
            }
        }

        [Application]
        internal class AppB
        {
            [StateReport]
            public StateReport AnotherStateSampleFunction()
            {
                return new StateReport()
                {
                    Status = Status.Finished,
                };
            }
        }

        [Application]
        internal class AppC
        {
        }

        internal class AppD
        {
        }

        [Application]
        internal class AppE
        {
            [Property("Test")]
            public ComplexProperties ComplexProperties { get; set; }
        }

        internal class ComplexProperties
        {
            public string PropertyA { get; set; }

            public string PropertyB { get; set; }

            public SubProperties SubPropertiesA { get; set; }

            public SubProperties SubPropertiesB { get; set; }
        }

        internal class SubProperties
        {
            public string SampleSubPropertyA { get; set; }

            public string SampleSubPropertyB { get; set; }
        }
    }
}