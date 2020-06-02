namespace DG.Core.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using DG.Core.Applications.InMemoryHosting;
    using DG.Core.Attributes;
    using DG.Core.Model.Enums;
    using DG.Core.Model.Output;
    using DG.Core.Orchestrators;
    using DG.Core.Scanners;
    using DG.Core.Tests.Collections;
    using Moq;
    using Xunit;

    public class ApplicationOrchestratorWithInMemoryHostTests
    {
        [Theory]
        [InlineData("AppA", "instanceA")]
        [InlineData("AppB", "instanceA")]
        public void ShouldGetInstanceStateInCaseOfTypedDataInStateReport(string applicationName, string instanceName)
        {
            //// Arrange
            //var inMemoryOrchestrator = this.BuildApplicationOrchestrator();

            //// Act
            //inMemoryOrchestrator.Register(applicationName, instanceName);
            //inMemoryOrchestrator.BuildInstance(applicationName, instanceName, string.Empty);
            //var reports = inMemoryOrchestrator.GetInstanceState(applicationName, instanceName).ToList();

            //// Assert
            //reports.Should().HaveCount(1);
            //reports.First().Should().Match<StateReport>(x => x.Status == Status.Finished);
        }

        [Theory]
        [InlineData("AppC", "instanceA")]
        [InlineData("AppD", "instanceA")]
        public void ShouldThrowExceptionInGetInstanceStateInCaseOfMissingStateReport(
            string applicationName,
            string instanceName)
        {
            //// Arrange
            //var inMemoryOrchestrator = this.BuildApplicationOrchestrator();

            //// Act
            //inMemoryOrchestrator.Register(applicationName, instanceName);
            //inMemoryOrchestrator.BuildInstance(applicationName, instanceName, string.Empty);
            //var report = Assert.Throws<ArgumentOutOfRangeException>(() =>
            //    inMemoryOrchestrator.GetInstanceState(applicationName, instanceName).ToList());

            //// Assert
            //report.Should().BeOfType<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ShouldThrowExceptionInGetInstanceStateInCaseOfMissingApplications()
        {
            //// Arrange
            //var inMemoryOrchestrator = this.BuildApplicationOrchestrator();

            //// Act
            //var reportsC = Assert.Throws<KeyNotFoundException>(() =>
            //    inMemoryOrchestrator.GetInstanceState("testAppC", "instanceA").ToList());

            //// Assert
            //reportsC.Should().BeOfType<KeyNotFoundException>();
        }

        private IApplicationBuilder BuildInMemoryApplicationBuilder(params object[] dependencies)
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
                dependencies.GetOrMock<IApplicationSettingsWriter>()
                );
        }

        private ApplicationOrchestrator BuildApplicationOrchestrator(params object[] dependencies)
        {
            throw new NotImplementedException();
            //var inMemoryHost = new InMemoryApplicationHost(
            //    dependencies.GetOrMock<IApplicationController>(),
            //    dependencies.GetOrMock<IApplicationBuilder>(),
            //    dependencies.GetOrMock<IApplicationSettingsReader>());

            //var inMemoryOrchestrator = new ApplicationOrchestrator(new List<IApplicationHost>()
            //{
            //    inMemoryHost,
            //});

            //return inMemoryOrchestrator;
        }

        private ApplicationInfo BuildApplicationInfo(string applicationName, string instanceName, string hostingModel = "InMemory", int instanceCount = 1, string propertiesAsJson = "{}")
        {
            return new ApplicationInfo()
            {
                ApplicationUniqueId = new ApplicationUniqueId()
                {
                    Application = applicationName,
                    InstanceName = instanceName,
                },
                InstanceCount = instanceCount,
                PropertiesAsJson = string.Empty,
                HostingModel = hostingModel,
            };
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