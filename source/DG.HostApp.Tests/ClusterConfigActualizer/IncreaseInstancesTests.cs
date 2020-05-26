namespace DG.HostApp.Tests.ClusterConfigActualizer
{
    using DG.Core.Model.ClusterConfig;
    using DG.Core.Orchestrators;
    using DG.HostApp.Services.ClusterConfigActualizer;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class IncreaseInstancesTests
    {
        [Fact]
        public void ShouldReturnFalseOnCanExecuteCallWithNegativeDifference()
        {
            // Arrange
            int negativeDifference = -1;
            var applicationOrchestratorMock = new Mock<IApplicationOrchestrator>();
            var sut = new IncreaseInstances(applicationOrchestratorMock.Object);

            // Act
            var result = sut.CanExecute(negativeDifference);

            // Assert
            result.Should().Be(false);
        }

        [Fact]
        public void ShouldReturnTrueOnCanExecuteCallWithPositiveDifference()
        {
            // Arrange
            int positiveDifference = 42;
            var applicationOrchestratorMock = new Mock<IApplicationOrchestrator>();
            var sut = new IncreaseInstances(applicationOrchestratorMock.Object);

            // Act
            var result = sut.CanExecute(positiveDifference);

            // Assert
            result.Should().Be(true);
        }

        [Fact]
        public void ShouldCallBuildInstanceOnExecute()
        {
            // Arrange
            int positiveDifference = 42;
            var applicationOrchestratorMock = new Mock<IApplicationOrchestrator>();
            var testTypeString = "TestType";
            var testNameString = "TestName";
            var testSettingsString = "TestSettings";

            var applicationInstanceMock = new ApplicationInstance()
            {
                Type = "TestType",
                Name = "TestName",
                Settings = (object)"TestSettings",
            };

            var sut = new IncreaseInstances(applicationOrchestratorMock.Object);

            // Act
            sut.Execute(applicationInstanceMock, positiveDifference);

            // Assert
            applicationOrchestratorMock.Verify(
                x => x.BuildInstance(testTypeString, testNameString, testSettingsString, positiveDifference), Times.Once());
        }
    }
}
