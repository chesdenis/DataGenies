using DG.Core.Applications.InMemoryHosting;
using DG.Core.Attributes;
using DG.Core.Extensions;
using DG.Core.Model.Enums;
using DG.Core.Model.Output;
using DG.Core.Orchestrators;
using DG.Core.Scanners;
using DG.Core.Tests.Collections;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DG.Core.Tests.Unit.Applications.InMemoryHosting
{
    public class InMemoryApplicationBuilderTests
    {
        [Theory]
        [InlineData("AppA", "instanceA")]
        [InlineData("AppB", "instanceA")]
        public void ShouldThrowExceptionIfWeTryToBuildMoreThanOne(string applicationName, string instanceName)
        {
            // Arrange
            var appUniqueId = ApplicationExtensions.ParseUniqueId(applicationName, instanceName);
            var inMemoryApplications = new InMemoryApplications();

            // Act
            var sut = this.BuildSut(inMemoryApplications);
            sut.Build(appUniqueId, this.GetExampleSettingsAsJson());

            // Assert
            Assert.Throws<ArgumentException>(() => sut.Build(appUniqueId, this.GetExampleSettingsAsJson()));
        }

        [Theory]
        [InlineData("AppA", "instanceA")]
        [InlineData("AppB", "instanceA")]
        public void ShouldThrowExceptionIfWeTryToRemoveMoreThanOne(string applicationName, string instanceName)
        {
            // Arrange
            var appUniqueId = ApplicationExtensions.ParseUniqueId(applicationName, instanceName);
            var inMemoryApplications = new InMemoryApplications();

            // Act
            var sut = this.BuildSut(inMemoryApplications);
            sut.Build(appUniqueId, this.GetExampleSettingsAsJson());
            sut.Remove(appUniqueId);

            // Assert
            Assert.Throws<ArgumentException>(() => sut.Remove(appUniqueId));
        }

        [Fact]
        public void ShouldSupportMultipleApplicationInstancesAtSameTime()
        {
            // Arrange
            var inMemoryApplications = new InMemoryApplications();
            var sut = this.BuildSut(inMemoryApplications);

            var appAUniqueId = ApplicationExtensions.ParseUniqueId("AppA", "instanceA");
            var appBUniqueId = ApplicationExtensions.ParseUniqueId("AppB", "instanceB");

            // Act
            sut.Build(appAUniqueId);
            sut.Build(appBUniqueId);
            sut.Scale(appAUniqueId, 10);
            sut.Scale(appBUniqueId, 15);

            // Assert
            inMemoryApplications.Should().HaveCount(2);

            inMemoryApplications[appAUniqueId].Instances.Should().HaveCount(10);
            inMemoryApplications[appAUniqueId].Instances.First().Should()
                .BeOfType(typeof(AppA));

            inMemoryApplications[appBUniqueId].Instances.Should().HaveCount(15);
            inMemoryApplications[appBUniqueId].Instances.First().Should()
                .BeOfType(typeof(AppB));
        }

        [Fact]
        public void ShouldRemoveMultipleApplicationInstancesAtSameTime()
        {
            // Arrange
            var inMemoryApplications = new InMemoryApplications();
            var sut = this.BuildSut(inMemoryApplications);

            var appAUniqueId = ApplicationExtensions.ParseUniqueId("AppA", "instanceA");
            var appBUniqueId = ApplicationExtensions.ParseUniqueId("AppB", "instanceB");

            // Act
            sut.Build(appAUniqueId);
            sut.Build(appBUniqueId);

            sut.Scale(appAUniqueId, 10);
            sut.Scale(appBUniqueId, 15);

            sut.Remove(appAUniqueId);
            sut.Remove(appBUniqueId);

            // Assert
            inMemoryApplications.Should().HaveCount(0);
        }

        [Fact]
        public void ShouldStopAndStartApplicationsOnScaling()
        {
            // Arrange
            var inMemoryApplications = new InMemoryApplications();
            var applicationController = new Mock<IApplicationController>();
            var sut = this.BuildSut(inMemoryApplications, applicationController.Object);

            var appAUniqueId = ApplicationExtensions.ParseUniqueId("AppA", "instanceA");
            var appBUniqueId = ApplicationExtensions.ParseUniqueId("AppB", "instanceB");

            // Act
            sut.Build(appAUniqueId);
            sut.Build(appBUniqueId);

            sut.Scale(appAUniqueId, 10);
            sut.Scale(appBUniqueId, 15);
            
            // Assert
            inMemoryApplications.Should().HaveCount(2);

            applicationController.Verify(
               x =>
                   x.Stop(
                       It.Is<ApplicationUniqueId>(w => w.Equals(appAUniqueId))),
               Times.Exactly(2));

            applicationController.Verify(
              x =>
                  x.Stop(
                      It.Is<ApplicationUniqueId>(w => w.Equals(appBUniqueId))),
              Times.Exactly(2));

            applicationController.Verify(
               x =>
                   x.Start(
                       It.Is<ApplicationUniqueId>(w => w.Equals(appAUniqueId))),
               Times.Exactly(2));

            applicationController.Verify(
              x =>
                  x.Start(
                      It.Is<ApplicationUniqueId>(w => w.Equals(appBUniqueId))),
              Times.Exactly(2));
        }

        [Theory]
        [InlineData("AppA", "instanceA", typeof(AppA))]
        [InlineData("AppB", "instanceA", typeof(AppB))]
        public void ShouldCreateApplicationInstance(string applicationName, string instanceName, Type expectedType)
        {
            // Arrange
            var inMemoryApplications = new InMemoryApplications();
            var sut = this.BuildSut(inMemoryApplications);

            var appUniqueId = ApplicationExtensions.ParseUniqueId(applicationName, instanceName);

            // Act
            sut.Build(appUniqueId);

            // Assert
            inMemoryApplications.Should().HaveCount(1)
                .And.Subject[appUniqueId].Instances.First().Should()
                .BeOfType(expectedType);
        }

        [Theory]
        [InlineData("AppA", "instanceNameWith//DoubleBackslashes")]
        [InlineData("AppA", "instanceNameWith/Backslash")]
        [InlineData("AppA", "instanceNameWith'Ampersand")]
        [InlineData("AppA", "instanceNameWith\\Slash")]
        public void ShouldBuildApplicationsWithSpecializedNames(string applicationName, string instanceName)
        {
            // Arrange
            var inMemoryApplications = new InMemoryApplications();
            var sut = this.BuildSut(inMemoryApplications);

            var appUniqueId = ApplicationExtensions.ParseUniqueId(applicationName, instanceName);

            // Act
            sut.Build(appUniqueId);

            // Assert
            inMemoryApplications.Should().HaveCount(1)
               .And.Subject[appUniqueId].Instances.First().Should()
               .BeOfType(typeof(AppA));
        }

        [Theory]
        [InlineData("AppC", "instanceA")]
        [InlineData("AppD", "instanceD")]
        [InlineData("AppE", "instanceE")]
        public void ShouldWriteSettingsDuringBuildOfApplicationInstance(string applicationName, string instanceName)
        {
            // Arrange
            var inMemoryApplications = new InMemoryApplications();
            var applicationSettingsWriter = new Mock<IApplicationSettingsWriter>();
            var sut = this.BuildSut(inMemoryApplications, applicationSettingsWriter.Object);

            var appUniqueId = ApplicationExtensions.ParseUniqueId(applicationName, instanceName);

            // Act
            sut.Build(appUniqueId, this.GetExampleSettingsAsJson());

            // Assert
            applicationSettingsWriter.Verify(
                x =>
                    x.WriteSettings(It.IsAny<object>(), It.IsAny<string>()),
                Times.Once);
        }


        private InMemoryApplicationBuilder BuildSut(params object[] dependencies)
        {
            var applicationScannerMock = new Mock<IApplicationTypesScanner>();
            applicationScannerMock.Setup(x => x.Scan()).Returns(new List<Type>
            {
                typeof(AppA),
                typeof(AppB),
                typeof(AppC),
                typeof(AppD),
                typeof(AppE),
            });

            return new InMemoryApplicationBuilder(
                dependencies.GetOrMock<InMemoryApplications>(isExplicit: true),
                dependencies.GetOrMock<IApplicationController>(),
                applicationScannerMock.Object,
                dependencies.GetOrMock<IApplicationSettingsWriter>());
        }

        private string GetExampleSettingsAsJson()
        {
            return @"{
        ""Settings"": {
            ""B"": ""This is simple property B"",
            ""RegularC"": {
                ""A"": ""This is regular sub property A"",
                ""B"": ""This is regular sub property B""
            },
            ""F"": 1234
        }
}";
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
            public SampleSettings Settings { get; set; }
        }

        internal class SampleSettings
        {
            public string B { get; set; }

            public RegularComplexC RegularC { get; set; }

            public int F { get; set; }
        }

        internal class RegularComplexC
        {
            public string A { get; set; }

            public string B { get; set; }
        }
    }
}
