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
        public void ShouldStopEveryInstanceThatIsNotAssignToHost()
        {
            // Arrange
            var clusterConfigManagerMock = new Mock<IClusterConfigManager>();
            var applicationOrchestratorMock = new Mock<IApplicationOrchestrator>();
            var increaseInstancesMock = new Mock<IncreaseInstances>();
            var decreaseInstancesMock = new Mock<DecreaseInstances>();
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
                            PlacementPolicies = new List<string>() {"FirstHost","SecondHost","ThirdHost" },
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
            instances.Add("", new List<object>() { new object() });
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
            applicationOrchestratorMock(aom => aom.Stop(), Times.Once());
            applicationOrchestratorMock(aom => aom.Stop(), Times.Once());
            applicationOrchestratorMock(aom => aom.UnRegister(), Times.Once());
            applicationOrchestratorMock(aom => aom.UnRegister(), Times.Once());
        }
    }
}
