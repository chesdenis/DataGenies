namespace DG.HostApp.Tests.ClusterConfigActualizer
{
    using System.Collections;
    using System.Collections.Generic;
    using DG.Core.Model.ClusterConfig;
    using DG.HostApp.Services.ClusterConfigActualizer;
    using FluentAssertions;
    using Xunit;

    public class InstanceDifferenceCalculatorTests
    {
        [Theory]
        [ClassData(typeof(ShouldCallBuildInstanceOnExecuteData))]
        public void ShouldCallBuildInstanceOnExecute(
            ApplicationInstance applicationInstance,
            Host currentHost,
            int existingInstanceAmount,
            int expectingResult)
        {
            // Arrange
            var sut = new InstanceDifferenceCalculator();

            // Act
            var result = sut.CalculateDifference(applicationInstance, currentHost, existingInstanceAmount);

            // Assert 
            result.Should().Be(expectingResult);
        }

        public class ShouldCallBuildInstanceOnExecuteData : IEnumerable<object[]>
        {

            private static ApplicationInstance firstApplicationInstance = new ApplicationInstance()
            {
                PlacementPolicies = new List<string>() { "FirstHost", "SecondHost" },
                Count = "3",
            };

            private static ApplicationInstance secondApplicationInstance = new ApplicationInstance()
            {
                PlacementPolicies = new List<string>() { "FirstHost", "SecondHost" },
                Count = "13",
            };

            private static ApplicationInstance thirdApplicationInstance = new ApplicationInstance()
            {
                PlacementPolicies = new List<string>() { "FirstHost", "SecondHost", "ThirdHost" },
                Count = "5",
            };

            private static ApplicationInstance fourthApplicationInstance = new ApplicationInstance()
            {
                PlacementPolicies = new List<string>() { "FirstHost", "SecondHost", "ThirdHost", "FourthHost" },
                Count = "3",
            };

            private static Host firstCurrentHost = new Host() { Name = "FirstHost" };

            private static Host secondCurrentHost = new Host() { Name = "SecondHost" };

            private static Host thirdCurrentHost = new Host() { Name = "ThirdHost" };

            private static Host fourthCurrentHost = new Host() { Name = "FourthHost" };

            private readonly List<object[]> data = new List<object[]>
                {
                    new object[] { firstApplicationInstance, firstCurrentHost, 0, 2 },
                    new object[] { firstApplicationInstance, firstCurrentHost, 1, 1 },
                    new object[] { firstApplicationInstance, secondCurrentHost, 0, 1 },
                    new object[] { firstApplicationInstance, secondCurrentHost, 10, -9 },
                    new object[] { secondApplicationInstance, firstCurrentHost, 0, 7 },
                    new object[] { secondApplicationInstance, secondCurrentHost, 2, 4 },
                    new object[] { secondApplicationInstance, firstCurrentHost, 20, -13 },
                    new object[] { secondApplicationInstance, secondCurrentHost, 10, -4 },
                    new object[] { thirdApplicationInstance, firstCurrentHost, 0, 2 },
                    new object[] { thirdApplicationInstance, secondCurrentHost, 13, -11 },
                    new object[] { thirdApplicationInstance, thirdCurrentHost, 0, 1 },
                    new object[] { fourthApplicationInstance, firstCurrentHost, 10, -9 },
                    new object[] { fourthApplicationInstance, secondCurrentHost, 10, -9 },
                    new object[] { fourthApplicationInstance, thirdCurrentHost, 10, -9 },
                    new object[] { fourthApplicationInstance, fourthCurrentHost, 0, 0 },
                };

            public IEnumerator<object[]> GetEnumerator()
            { return this.data.GetEnumerator(); }

            IEnumerator IEnumerable.GetEnumerator()
            { return this.GetEnumerator(); }
        }
    }
}
