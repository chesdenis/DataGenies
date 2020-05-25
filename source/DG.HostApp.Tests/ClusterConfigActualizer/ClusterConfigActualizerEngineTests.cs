namespace DG.HostApp.Tests.ClusterConfigActualizer
{
    using System.Collections.Generic;
    using DG.Core.ConfigManagers;
    using DG.Core.Model.ClusterConfig;
    using DG.Core.Orchestrators;
    using DG.HostApp.Services.ClusterConfigActualizer;
    using Moq;
    using Xunit;

    public class ClusterConfigActualizerEngineTests
    {
        [Fact]
        public void ShouldStopAndUnregisterEveryInstanceThatIsNotDefinedInClusterConfig()
        {
            // Arrange
            var clusterConfigManagerMock = new Mock<IClusterConfigManager>();
            var applicationOrchestratorMock = new Mock<IApplicationOrchestrator>();
            var increaseInstancesMock = new Mock<IInstanceActions>();
            var decreaseInstancesMock = new Mock<IInstanceActions>();
            var instanceActionsMock = new List<IInstanceActions>() { increaseInstancesMock.Object, decreaseInstancesMock.Object };
            var instanceDifferenceCalculatorMock = new Mock<IInstanceDifferenceCalculator>();

            var clusterConfig = new ClusterConfig()
            {
                ClusterDefinition = new ClusterDefinition()
                {
                    ApplicationInstances = new ApplicationInstances()
                    {
                        new ApplicationInstance()
                        {
                            Name = "FirstTestName",
                            Type = "FirstTestType",
                            PlacementPolicies = new List<string>() { "FirstHost", "SecondHost", "ThirdHost" },
                            Count = "3",
                        },
                        new ApplicationInstance()
                        {
                            Name = "SecondTestName",
                            Type = "SecondTestType",
                            PlacementPolicies = new List<string>() { "FirstHost", "SecondHost", "ThirdHost" },
                            Count = "4",
                        },
                    },
                },
            };

            var currentHost = new Host()
            {
                Name = "FancyHostName",
            };

            var instances = new Dictionary<string, List<object>>();
            instances.Add("InstanceType/FirstInstance", new List<object>() { new object() });
            instances.Add("InstanceType/SecondInstance", new List<object>() { new object() });
            clusterConfigManagerMock.Setup(ccm => ccm.GetConfig()).Returns(clusterConfig);
            clusterConfigManagerMock.Setup(ccm => ccm.GetHost()).Returns(currentHost);
            applicationOrchestratorMock.Setup(aom => aom.GetInMemoryInstancesData()).Returns(instances);

            var sut = new ClusterConfigActualizerEngine(
                clusterConfigManagerMock.Object,
                applicationOrchestratorMock.Object,
                instanceActionsMock,
                instanceDifferenceCalculatorMock.Object);

            // Act
            sut.Actualize();

            // Assert
            applicationOrchestratorMock.Verify(aom => aom.Stop(It.Is<string>(s => s.Contains("InstanceType/FirstInstance"))), Times.Once());
            applicationOrchestratorMock.Verify(aom => aom.Stop(It.Is<string>(s => s.Contains("InstanceType/SecondInstance"))), Times.Once());
            applicationOrchestratorMock.Verify(aom => aom.UnRegister(It.Is<string>(s => s.Contains("InstanceType/FirstInstance"))), Times.Once());
            applicationOrchestratorMock.Verify(aom => aom.UnRegister(It.Is<string>(s => s.Contains("InstanceType/SecondInstance"))), Times.Once());
        }

        [Fact]
        public void ShouldStopAndUnRegisterAllInstancesIfTheyAreNotAssignedToCurrentHost()
        {
            // Arrange
            var clusterConfigManagerMock = new Mock<IClusterConfigManager>();
            var applicationOrchestratorMock = new Mock<IApplicationOrchestrator>();
            var increaseInstancesMock = new Mock<IInstanceActions>();
            var decreaseInstancesMock = new Mock<IInstanceActions>();
            var instanceActionsMock = new List<IInstanceActions>() { increaseInstancesMock.Object, decreaseInstancesMock.Object };
            var instanceDifferenceCalculatorMock = new Mock<IInstanceDifferenceCalculator>();

            var clusterConfig = new ClusterConfig()
            {
                ClusterDefinition = new ClusterDefinition()
                {
                    ApplicationInstances = new ApplicationInstances()
                    {
                        new ApplicationInstance()
                        {
                            Name = "FirstTestName",
                            Type = "FirstTestType",
                            PlacementPolicies = new List<string>() { "FirstHost", "SecondHost", "ThirdHost" },
                            Count = "3",
                        },
                        new ApplicationInstance()
                        {
                            Name = "SecondTestName",
                            Type = "SecondTestType",
                            PlacementPolicies = new List<string>() { "FirstHost", "SecondHost", "ThirdHost" },
                            Count = "4",
                        },
                    },
                },
            };

            var currentHost = new Host()
            {
                Name = "FancyHostName",
            };

            var instances = new Dictionary<string, List<object>>();
            instances.Add("FirstTestType/FirstTestName", new List<object>() { new object() });
            instances.Add("SecondTestType/SecondTestName", new List<object>() { new object() });
            clusterConfigManagerMock.Setup(ccm => ccm.GetConfig()).Returns(clusterConfig);
            clusterConfigManagerMock.Setup(ccm => ccm.GetHost()).Returns(currentHost);
            applicationOrchestratorMock.Setup(aom => aom.GetInMemoryInstancesData()).Returns(instances);

            var sut = new ClusterConfigActualizerEngine(
                clusterConfigManagerMock.Object,
                applicationOrchestratorMock.Object,
                instanceActionsMock,
                instanceDifferenceCalculatorMock.Object);

            // Act
            sut.Actualize();

            // Assert
            applicationOrchestratorMock.Verify(aom => aom.Stop(It.Is<string>(s => s.Contains("FirstTestType/FirstTestName"))), Times.Once());
            applicationOrchestratorMock.Verify(aom => aom.Stop(It.Is<string>(s => s.Contains("SecondTestType/SecondTestName"))), Times.Once());
            applicationOrchestratorMock.Verify(aom => aom.UnRegister(It.Is<string>(s => s.Contains("FirstTestType/FirstTestName"))), Times.Once());
            applicationOrchestratorMock.Verify(aom => aom.UnRegister(It.Is<string>(s => s.Contains("SecondTestType/SecondTestName"))), Times.Once());
        }

        [Fact]
        public void ShouldCallIncreaseInstance()
        {
            // Arrange
            var clusterConfigManagerMock = new Mock<IClusterConfigManager>();
            var applicationOrchestratorMock = new Mock<IApplicationOrchestrator>();
            var increaseInstancesMock = new Mock<IInstanceActions>();
            var decreaseInstancesMock = new Mock<IInstanceActions>();
            var instanceActionsMock = new List<IInstanceActions>() { increaseInstancesMock.Object, decreaseInstancesMock.Object };
            var instanceDifferenceCalculatorMock = new Mock<IInstanceDifferenceCalculator>();

            var clusterConfig = new ClusterConfig()
            {
                ClusterDefinition = new ClusterDefinition()
                {
                    ApplicationInstances = new ApplicationInstances()
                    {
                        new ApplicationInstance()
                        {
                            Name = "FirstTestName",
                            Type = "FirstTestType",
                            PlacementPolicies = new List<string>() { "FirstHost", "SecondHost", "ThirdHost" },
                            Count = "7",
                        },
                        new ApplicationInstance()
                        {
                            Name = "SecondTestName",
                            Type = "SecondTestType",
                            PlacementPolicies = new List<string>() { "FirstHost", "SecondHost", "ThirdHost" },
                            Count = "4",
                        },
                    },
                },
            };

            var currentHost = new Host()
            {
                Name = "FirstHost",
            };

            var instances = new Dictionary<string, List<object>>();
            instances.Add("FirstTestType/FirstTestName", new List<object>() { new object() });
            instances.Add("SecondTestType/SecondTestName", new List<object>() { new object() });
            clusterConfigManagerMock.Setup(ccm => ccm.GetConfig()).Returns(clusterConfig);
            clusterConfigManagerMock.Setup(ccm => ccm.GetHost()).Returns(currentHost);
            applicationOrchestratorMock.Setup(aom => aom.GetInMemoryInstancesData()).Returns(instances);
            increaseInstancesMock.Setup(iim => iim.CanExecute(It.IsAny<int>())).Returns(true);
            instanceDifferenceCalculatorMock
                .Setup(idc => idc.CalculateDifference(
                    It.IsAny<ApplicationInstance>(),
                    It.IsAny<Host>(),
                    It.IsAny<int>()))
                .Returns(4);

            var sut = new ClusterConfigActualizerEngine(
                clusterConfigManagerMock.Object,
                applicationOrchestratorMock.Object,
                instanceActionsMock,
                instanceDifferenceCalculatorMock.Object);

            // Act
            sut.Actualize();

            // Assert
            increaseInstancesMock.Verify(
                iim => iim.Execute(
                    It.Is<ApplicationInstance>(ai => ai.Name == "FirstTestName" & ai.Type == "FirstTestType"),
                    It.Is<int>(i => i == 4)), Times.Once());
        }

        [Fact]
        public void ShouldCallDecreaseInstance()
        {
            // Arrange
            var clusterConfigManagerMock = new Mock<IClusterConfigManager>();
            var applicationOrchestratorMock = new Mock<IApplicationOrchestrator>();
            var increaseInstancesMock = new Mock<IInstanceActions>();
            var decreaseInstancesMock = new Mock<IInstanceActions>();
            var instanceActionsMock = new List<IInstanceActions>() { increaseInstancesMock.Object, decreaseInstancesMock.Object };
            var instanceDifferenceCalculatorMock = new Mock<IInstanceDifferenceCalculator>();

            var clusterConfig = new ClusterConfig()
            {
                ClusterDefinition = new ClusterDefinition()
                {
                    ApplicationInstances = new ApplicationInstances()
                    {
                        new ApplicationInstance()
                        {
                            Name = "FirstTestName",
                            Type = "FirstTestType",
                            PlacementPolicies = new List<string>() { "FirstHost", "SecondHost", "ThirdHost" },
                            Count = "7",
                        },
                        new ApplicationInstance()
                        {
                            Name = "SecondTestName",
                            Type = "SecondTestType",
                            PlacementPolicies = new List<string>() { "FirstHost", "SecondHost", "ThirdHost" },
                            Count = "4",
                        },
                    },
                },
            };

            var currentHost = new Host()
            {
                Name = "FirstHost",
            };

            var instances = new Dictionary<string, List<object>>();
            instances.Add("FirstTestType/FirstTestName", new List<object>() { new object() });
            instances.Add("SecondTestType/SecondTestName", new List<object>() { new object() });
            clusterConfigManagerMock.Setup(ccm => ccm.GetConfig()).Returns(clusterConfig);
            clusterConfigManagerMock.Setup(ccm => ccm.GetHost()).Returns(currentHost);
            applicationOrchestratorMock.Setup(aom => aom.GetInMemoryInstancesData()).Returns(instances);
            decreaseInstancesMock.Setup(iim => iim.CanExecute(It.IsAny<int>())).Returns(true);
            instanceDifferenceCalculatorMock
                .Setup(idc => idc.CalculateDifference(
                    It.IsAny<ApplicationInstance>(),
                    It.IsAny<Host>(),
                    It.IsAny<int>()))
                .Returns(-4);

            var sut = new ClusterConfigActualizerEngine(
                clusterConfigManagerMock.Object,
                applicationOrchestratorMock.Object,
                instanceActionsMock,
                instanceDifferenceCalculatorMock.Object);

            // Act
            sut.Actualize();

            // Assert
            decreaseInstancesMock.Verify(
                iim => iim.Execute(
                    It.Is<ApplicationInstance>(ai => ai.Name == "FirstTestName" & ai.Type == "FirstTestType"),
                    It.Is<int>(i => i == -4)), Times.Once());
        }

        [Fact]
        public void ShouldBuildInstanceIfItIsDefineInClusterConfig()
        {
            // Arrange
            var clusterConfigManagerMock = new Mock<IClusterConfigManager>();
            var applicationOrchestratorMock = new Mock<IApplicationOrchestrator>();
            var increaseInstancesMock = new Mock<IInstanceActions>();
            var decreaseInstancesMock = new Mock<IInstanceActions>();
            var instanceActionsMock = new List<IInstanceActions>() { increaseInstancesMock.Object, decreaseInstancesMock.Object };
            var instanceDifferenceCalculatorMock = new Mock<IInstanceDifferenceCalculator>();

            var clusterConfig = new ClusterConfig()
            {
                ClusterDefinition = new ClusterDefinition()
                {
                    ApplicationInstances = new ApplicationInstances()
                    {
                        new ApplicationInstance()
                        {
                            Name = "FirstTestName",
                            Type = "FirstTestType",
                            Settings = (object) "SettingsString",
                            PlacementPolicies = new List<string>() { "FirstHost", "SecondHost", "ThirdHost" },
                            Count = "3",
                        },
                        new ApplicationInstance()
                        {
                            Name = "SecondTestName",
                            Type = "SecondTestType",
                            PlacementPolicies = new List<string>() { "FirstHost", "SecondHost", "ThirdHost" },
                            Count = "4",
                        },
                    },
                },
            };

            var currentHost = new Host()
            {
                Name = "FirstHost",
            };

            var instances = new Dictionary<string, List<object>>();
            instances.Add("SomeType/SomeInstance", new List<object>() { new object() });
            clusterConfigManagerMock.Setup(ccm => ccm.GetConfig()).Returns(clusterConfig);
            clusterConfigManagerMock.Setup(ccm => ccm.GetHost()).Returns(currentHost);
            applicationOrchestratorMock.Setup(aom => aom.GetInMemoryInstancesData()).Returns(instances);
            instanceDifferenceCalculatorMock
                .Setup(idc => idc.CalculateDifference(
                    It.IsAny<ApplicationInstance>(),
                    It.IsAny<Host>(),
                    It.IsAny<int>()))
                .Returns(4);

            var sut = new ClusterConfigActualizerEngine(
                clusterConfigManagerMock.Object,
                applicationOrchestratorMock.Object,
                instanceActionsMock,
                instanceDifferenceCalculatorMock.Object);

            // Act
            sut.Actualize();

            // Assert
            applicationOrchestratorMock.Verify(
                iim => iim.BuildInstance(
                    It.Is<string>(s => s == "FirstTestType"),
                    It.Is<string>(s => s == "FirstTestName"),
                    It.Is<string>(s => s == "SettingsString"),
                    It.Is<int>(i => i == 4)), Times.Once());
        }
    }
}
