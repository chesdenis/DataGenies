namespace DG.Core.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using DG.Core.ConfigManagers;
    using DG.Core.Models;
    using DG.Core.Repositories;
    using DG.Core.Services;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class JsonClusterConfigManagerTests
    {
        [Fact]
        public void ShouldGetProperClusterConfigAsJson()
        {
            // Arrange
            var clusterConfigRepositoryMock = new Mock<IClusterConfigRepository>();
            clusterConfigRepositoryMock.Setup(x => x.GetClusterConfig()).Returns(this.fakeClusterConfig);
            var systemClockMock = new SystemClockMock();

            var sut = new JsonClusterConfigManager(clusterConfigRepositoryMock.Object, systemClockMock);

            // Act
            var result = sut.GetConfigAsJson();

            // Assert
            result.Should().Be(this.fakeClusterConfigAsJson);
        }

        [Fact]
        public void ShouldWriteProperSignedClusterConfigObject()
        {
            var clusterConfigRepositoryMock = new Mock<IClusterConfigRepository>();
            clusterConfigRepositoryMock.Setup(x => x.GetClusterConfig()).Returns(this.fakeClusterConfig);
            var systemClockMock = new SystemClockMock();

            var sut = new JsonClusterConfigManager(clusterConfigRepositoryMock.Object, systemClockMock);

            // Act
            sut.WriteConfig(this.fakeClusterConfig);

            // Assert
            clusterConfigRepositoryMock.Verify(x => x.UpdateClusterConfig(this.fakeClusterConfig), Times.Once());
        }

        private ClusterConfig fakeClusterConfig = new ClusterConfig()
        {
            LastUpdateTime = new DateTime(2020, 04, 26, 23, 50, 09),
            HashMD5 = "58DF38BD34418F93E6CC84A0658F4ADE",
            Nodes = new List<Node>()
                {
                    new Node()
                    {
                        NodeName = "TestNode",
                        HostName = "localhost",
                        HostingModel = "InMemory",
                        Port = 302,
                    },
                },
            ApplicationInstances = new List<ApplicationInstance>()
                {
                    new ApplicationInstance()
                    {
                        Name = "ApplicationName",
                        Type = "ApplicationType",
                        HostingModel = "InMemeory",
                        PlacementPolicies = new List<string>()
                        {
                            "TestPlacement",
                        },
                        Count = -273,
                    },
                },
        };

        private string fakeClusterConfigAsJson = @"{""LastUpdateTime"":""2020-04-26T23:50:09"",""HashMD5"":""58DF38BD34418F93E6CC84A0658F4ADE"",""Nodes"":[{""NodeName"":""TestNode"",""HostName"":""localhost"",""Port"":302,""HostingModel"":""InMemory""}],""ApplicationInstances"":[{""Name"":""ApplicationName"",""Type"":""ApplicationType"",""HostingModel"":""InMemeory"",""PlacementPolicies"":[""TestPlacement""],""Count"":-273}]}";

        private class SystemClockMock : ISystemClock
        {
            public DateTime Now { get { return new DateTime(2020, 04, 26, 23, 50, 09); } }
        }
    }
}