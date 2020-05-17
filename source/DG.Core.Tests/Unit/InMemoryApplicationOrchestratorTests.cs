using DG.Core.Writers;

namespace DG.Core.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DG.Core.Attributes;
    using DG.Core.Extensions;
    using DG.Core.Model.Enums;
    using DG.Core.Model.Output;
    using DG.Core.Orchestrators;
    using DG.Core.Scanners;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class InMemoryApplicationOrchestratorTests
    {
        [Theory]
        [InlineData("AppA", "instanceA")]
        [InlineData("AppB", "instanceA")]
        public void ShouldThrowExceptionIfWeTryToRegisterMoreThanOne(string applicationName, string instanceName)
        {
            // Arrange
            var inMemoryOrchestrator = this.BuildApplicationOrchestrator();

            // Act
            inMemoryOrchestrator.Register(applicationName, instanceName);
            inMemoryOrchestrator.BuildInstance(applicationName, instanceName, string.Empty);

            // Assert
            Assert.Throws<ArgumentException>(() => inMemoryOrchestrator.Register(applicationName, instanceName));
        }

        [Fact]
        public void ShouldSupportMultipleApplicationInstancesAtSameTime()
        {
            // Arrange
            var inMemoryOrchestrator = this.BuildApplicationOrchestrator();

            // Act
            inMemoryOrchestrator.Register("AppA", "instanceA");
            inMemoryOrchestrator.Register("AppB", "instanceB");
            inMemoryOrchestrator.BuildInstance("AppA", "instanceA", string.Empty, 10);
            inMemoryOrchestrator.BuildInstance("AppB", "instanceB", string.Empty, 15);

            // Assert
            var instancesData = inMemoryOrchestrator.GetInMemoryInstancesData();

            instancesData.Should().HaveCount(2);

            instancesData[ApplicationExtensions.ConstructUniqueId("AppA", "instanceA")].Should().HaveCount(10);
            instancesData[ApplicationExtensions.ConstructUniqueId("AppA", "instanceA")].First().Should()
                .BeOfType(typeof(AppA));

            instancesData[ApplicationExtensions.ConstructUniqueId("AppB", "instanceB")].Should().HaveCount(15);
            instancesData[ApplicationExtensions.ConstructUniqueId("AppB", "instanceB")].First().Should()
                .BeOfType(typeof(AppB));
        }

        [Theory]
        [InlineData("AppA", "instanceA", typeof(AppA))]
        [InlineData("AppB", "instanceA", typeof(AppB))]
        public void ShouldCreateApplicationInstance(string applicationName, string instanceName, Type expectedType)
        {
            // Arrange
            var inMemoryOrchestrator = this.BuildApplicationOrchestrator();

            // Act
            inMemoryOrchestrator.Register(applicationName, instanceName);
            inMemoryOrchestrator.BuildInstance(applicationName, instanceName, string.Empty);

            // Assert
            inMemoryOrchestrator.GetInMemoryInstancesData().Should().HaveCount(1)
                .And.Subject[ApplicationExtensions.ConstructUniqueId(applicationName, instanceName)].First().Should()
                .BeOfType(expectedType);
        }

        [Theory]
        [InlineData("AppA", "instanceNameWith//DoubleBackslashes")]
        [InlineData("AppA", "instanceNameWith/Backslash")]
        [InlineData("AppA", "instanceNameWith'Ampersand")]
        [InlineData("AppA", "instanceNameWith\\Slash")]
        public void ShouldWorkWithApplicationsWithSpecializedNames(string applicationName, string instanceName)
        {
            // Arrange
            var inMemoryOrchestrator = this.BuildApplicationOrchestrator();

            // Act
            inMemoryOrchestrator.Register(applicationName, instanceName);
            inMemoryOrchestrator.BuildInstance(applicationName, instanceName, string.Empty);
            var reports = inMemoryOrchestrator.GetInstanceState(applicationName, instanceName).ToList();

            // Assert
            reports.Should().HaveCount(1);
            reports.First().Should().Match<StateReport>(x => x.Status == Status.Finished);
        }

        [Theory]
        [InlineData("AppA", "instanceA")]
        [InlineData("AppB", "instanceA")]
        public void ShouldGetInstanceStateInCaseOfTypedDataInStateReport(string applicationName, string instanceName)
        {
            // Arrange
            var inMemoryOrchestrator = this.BuildApplicationOrchestrator();

            // Act
            inMemoryOrchestrator.Register(applicationName, instanceName);
            inMemoryOrchestrator.BuildInstance(applicationName, instanceName, string.Empty);
            var reports = inMemoryOrchestrator.GetInstanceState(applicationName, instanceName).ToList();

            // Assert
            reports.Should().HaveCount(1);
            reports.First().Should().Match<StateReport>(x => x.Status == Status.Finished);
        }

        [Theory]
        [InlineData("AppC", "instanceA")]
        [InlineData("AppD", "instanceA")]
        public void ShouldThrowExceptionInGetInstanceStateInCaseOfMissingStateReport(
            string applicationName,
            string instanceName)
        {
            // Arrange
            var inMemoryOrchestrator = this.BuildApplicationOrchestrator();

            // Act
            inMemoryOrchestrator.Register(applicationName, instanceName);
            inMemoryOrchestrator.BuildInstance(applicationName, instanceName, string.Empty);
            var report = Assert.Throws<ArgumentOutOfRangeException>(() =>
                inMemoryOrchestrator.GetInstanceState(applicationName, instanceName).ToList());

            // Assert
            report.Should().BeOfType<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData("AppC", "instanceA")]
        [InlineData("AppD", "instanceD")]
        [InlineData("AppE", "instanceE")]
        public void ShouldWriteSettingsDuringBuildOfApplicationInstance(string applicationName, string instanceName)
        {
            // Arrange
            var propertiesWriterMock = new Mock<IApplicationInstanceSettingsWriter>();
            var inMemoryOrchestrator = this.BuildApplicationOrchestrator(propertiesWriterMock);

            // Act
            inMemoryOrchestrator.Register(applicationName, instanceName);
            inMemoryOrchestrator.BuildInstance(applicationName, instanceName, "sample json data");

            // Assert
            propertiesWriterMock.Verify(
                x => 
                    x.WriteSettings(It.IsAny<object>(), It.IsAny<string>()), 
                Times.Once);
        }

        [Fact]
        public void ShouldThrowExceptionInGetInstanceStateInCaseOfMissingApplications()
        {
            // Arrange
            var inMemoryOrchestrator = this.BuildApplicationOrchestrator();

            // Act
            var reportsC = Assert.Throws<KeyNotFoundException>(() =>
                inMemoryOrchestrator.GetInstanceState("testAppC", "instanceA").ToList());

            // Assert
            reportsC.Should().BeOfType<KeyNotFoundException>();
        }
        
        private InMemoryApplicationOrchestrator BuildApplicationOrchestrator(Mock<IApplicationInstanceSettingsWriter> propertiesWriterMock = null)
        {
            propertiesWriterMock ??= new Mock<IApplicationInstanceSettingsWriter>();
            
            var applicationScannerMock = new Mock<IApplicationTypesScanner>();
            applicationScannerMock.Setup(x => x.Scan()).Returns(new List<Type>
            {
                typeof(AppA),
                typeof(AppB),
                typeof(AppC),
                typeof(AppD),
                typeof(AppE),
            });
            var inMemoryOrchestrator = new InMemoryApplicationOrchestrator(applicationScannerMock.Object, propertiesWriterMock.Object);
            inMemoryOrchestrator.CollectPossibleApplicationTypes();

            return inMemoryOrchestrator;
        }

        [Application]
        private class AppA
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
        private class AppB
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
        private class AppC
        {
        }

        private class AppD
        {
        }

        [Application]
        private class AppE
        {
            [Settings]
            public SettingsA Settings { get; set; }
        }
        
        private class SettingsA
        {
        }
    }
}