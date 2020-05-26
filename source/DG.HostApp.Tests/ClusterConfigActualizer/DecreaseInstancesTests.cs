namespace DG.HostApp.Tests.ClusterConfigActualizer
{
    using DG.Core.Model.ClusterConfig;
    using DG.Core.Orchestrators;
    using DG.HostApp.Services.ClusterConfigActualizer;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class DecreaseInstancesTests
    {
        [Fact]
        public void ShouldReturnTrueOnCanExecuteCallWithNegativeDifference()
        {
            // Arrange
            int negativeDifference = -1;
            var applicationOrchestratorMock = new Mock<IApplicationOrchestrator>();
            var sut = new DecreaseInstances(applicationOrchestratorMock.Object);

            // Act
            var result = sut.CanExecute(negativeDifference);

            // Assert
            result.Should().Be(true);
        }

        [Fact]
        public void ShouldReturnFalseOnCanExecuteCallWithPositiveDifference()
        {
            // Arrange
            int positiveDifference = 42;
            var applicationOrchestratorMock = new Mock<IApplicationOrchestrator>();
            var sut = new DecreaseInstances(applicationOrchestratorMock.Object);

            // Act
            var result = sut.CanExecute(positiveDifference);

            // Assert
            result.Should().Be(false);
        }

        [Fact]
        public void ShouldCallBuildInstanceOnExecute()
        {
            // Arrange
            int negativeDifference = -273;
            var applicationOrchestratorMock = new Mock<IApplicationOrchestrator>();
            var testTypeString = "TestType";
            var testNameString = "TestName";

            var applicationInstanceMock = new ApplicationInstance()
            {
                Type = "TestType",
                Name = "TestName",
            };

            var sut = new DecreaseInstances(applicationOrchestratorMock.Object);

            // Act
            sut.Execute(applicationInstanceMock, negativeDifference);

            // Assert
            applicationOrchestratorMock.Verify(
                x => x.RemoveInstance(testTypeString, testNameString, negativeDifference), Times.Once());
        }
    }
}
