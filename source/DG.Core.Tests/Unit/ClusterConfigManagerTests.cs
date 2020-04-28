using System;
using System.Collections.Generic;
using DG.Core.ConfigManagers;
using DG.Core.Extensions;
using DG.Core.Model.ClusterConfig;
using DG.Core.Repositories;
using DG.Core.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace DG.Core.Tests.Unit
{
    public class ClusterConfigManagerTests
    {
        [Fact]
        public void ShouldGetProperClusterConfig()
        {
            // Arrange
            var httpService = new Mock<IHttpService>();
            var configRepositoryMock = new Mock<IClusterConfigRepository>();
            var systemClockMock = new Mock<ISystemClock>();

            configRepositoryMock.Setup(x => x.GetClusterConfig()).Returns(this.GetSampleConfig);
            
            // Act
            var sut = new ClusterConfigManager(configRepositoryMock.Object, httpService.Object, systemClockMock.Object);
            var result = sut.GetConfig();
        
            // Assert
            result.CalculateMd5Hash().Should().Be(this.GetSampleConfig().CalculateMd5Hash());
        }
        
        [Fact]
        public void ShouldWriteProperSignedClusterConfigObject()
        {
            // Arrange
            var httpService = new Mock<IHttpService>();
            var configRepositoryMock = new Mock<IClusterConfigRepository>();
            configRepositoryMock.Setup(x => x.GetClusterConfig()).Returns(this.GetSampleConfig);
            var systemClockMock = new Mock<ISystemClock>();

            // Act
            var sut = new ClusterConfigManager(configRepositoryMock.Object, httpService.Object, systemClockMock.Object);
            var configToWrite = this.GetSampleConfig();
            sut.WriteConfig(configToWrite);
        
            // Assert
            configRepositoryMock.Verify(
                x => x.UpdateClusterConfig(
                It.Is<ClusterConfig>(
                    xx => xx.CalculateMd5Hash() == configToWrite.CalculateMd5Hash())), Times.Once());
        }
        
        [Fact]
        public void InCaseOfJsonErrorShouldFailIfMissingCorrectClusterConfigCache()
        {
            // Arrange
            var httpService = new Mock<IHttpService>();
            var configRepositoryMock = new Mock<IClusterConfigRepository>();
            var systemClockMock = new Mock<ISystemClock>();
            
            configRepositoryMock.Setup(x => x.GetClusterConfig())
                .Throws(new Exception("Can't read or deserialize cluster config"));
            
            // Act
            var sut = new ClusterConfigManager(configRepositoryMock.Object, httpService.Object, systemClockMock.Object);

            // Assert
            Assert.Throws<Exception>(() => sut.GetConfig());
        }
        
        [Fact]
        public void InCaseOfJsonErrorShouldUseClusterConfigCache()
        {
            // Arrange
            var httpService = new Mock<IHttpService>();
            var configRepositoryMock = new Mock<IClusterConfigRepository>();
            var systemClockMock = new Mock<ISystemClock>();
            var sampleConfig = this.GetSampleConfig();
            configRepositoryMock.Setup(x => x.GetClusterConfig())
                .Returns(sampleConfig);
            
            // Act
            var sut = new ClusterConfigManager(configRepositoryMock.Object, httpService.Object, systemClockMock.Object);
            var config = sut.GetConfig();
            configRepositoryMock.Setup(x => x.GetClusterConfig())
                .Throws(new Exception("Can't read or deserialize cluster config"));
            var configFromCache = sut.GetConfig();
            
            // Assert
            configFromCache.Should().NotBeNull();
        }

        [Fact]
        public void ShouldWriteHashStringInCaseOfAnyUpdateOfClusterDefinition()
        {
            // Arrange
            var httpService = new Mock<IHttpService>();
            var configRepositoryMock = new Mock<IClusterConfigRepository>();
            var systemClockMock = new Mock<ISystemClock>();
            
            var hostConfig = this.GetSampleConfig();

            configRepositoryMock.Setup(x =>
                x.UpdateClusterConfig(It.IsAny<ClusterConfig>()))
                .Callback<ClusterConfig>(x =>
            {
                hostConfig.ClusterDefinition = x.ClusterDefinition;
                hostConfig.CurrentHost = x.CurrentHost;
            });
           
            configRepositoryMock.Setup(x => x.GetClusterConfig()).Returns(hostConfig);
            
            // Act
            var sut = new ClusterConfigManager(configRepositoryMock.Object, httpService.Object, systemClockMock.Object);

            var originalHash = sut.GetConfig().ClusterDefinition.HashMD5;
            
            var configToUpdate = this.GetSampleConfig();
            
            configToUpdate.ClusterDefinition.Hosts.RemoveAt(0);
            sut.WriteClusterDefinition(configToUpdate.ClusterDefinition);
            var md5HashRemovedInstance = sut.GetConfig().ClusterDefinition.HashMD5;
            
            configToUpdate = this.GetSampleConfig();
            configToUpdate.ClusterDefinition.Hosts[0].Name += " - renamed";
            sut.WriteClusterDefinition(configToUpdate.ClusterDefinition);
            var md5HashRenamedInstance = sut.GetConfig().ClusterDefinition.HashMD5;
            
            // Assert
            originalHash.Should().NotBe(md5HashRemovedInstance);
            md5HashRemovedInstance.Should().NotBe(md5HashRenamedInstance);
        }

        [Fact]
        public void ShouldCallWriteConfigEndpointsForOtherHosts()
        {
            // Arrange
            var httpService = new Mock<IHttpService>();
            var configRepositoryMock = new Mock<IClusterConfigRepository>();
            var systemClockMock = new Mock<ISystemClock>();
            
            var hostConfig = this.GetSampleConfig();
            configRepositoryMock.Setup(x => x.GetClusterConfig()).Returns(hostConfig);
            
            // Act
            var sut = new ClusterConfigManager(configRepositoryMock.Object, httpService.Object, systemClockMock.Object);
            sut.SyncClusterDefinitionAcrossHosts();
            
            // Assert
            httpService.Verify(
                x => x.Post(
                    It.Is<string>(xx => xx == $"{hostConfig.ClusterDefinition.Hosts[1].GetClusterConfigManagerPublicEndpoint()}/WriteClusterDefinition"),
                    It.Is<string>(xx => xx.Contains("SampleAppInstanceA")), 
                    It.IsAny<string>()),
                Times.Once());
            
            httpService.Verify(
                x => x.Post(
                    It.Is<string>(xx => xx == $"{hostConfig.ClusterDefinition.Hosts[0].GetClusterConfigManagerPublicEndpoint()}/WriteClusterDefinition"),
                    It.Is<string>(xx => xx.Contains("SampleAppInstanceA")), 
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public void ShouldSkipWriteInCaseOfNoUpdateInClusterDefinition()
        {
            // Arrange
            var httpService = new Mock<IHttpService>();
            var configRepositoryMock = new Mock<IClusterConfigRepository>();
            var systemClockMock = new Mock<ISystemClock>();
            
            var hostConfig = this.GetSampleConfig();
            
            configRepositoryMock.Setup(x =>
                    x.UpdateClusterConfig(It.IsAny<ClusterConfig>()))
                .Callback<ClusterConfig>(x =>
                {
                    hostConfig.ClusterDefinition = x.ClusterDefinition;
                    hostConfig.CurrentHost = x.CurrentHost;
                });
           
            configRepositoryMock.Setup(x => x.GetClusterConfig()).Returns(hostConfig);
            
            // Act
            var sut = new ClusterConfigManager(configRepositoryMock.Object, httpService.Object, systemClockMock.Object);

            var configToUpdate = this.GetSampleConfig();
            configToUpdate.ClusterDefinition.Hosts[0].Name += " - renamed";
            sut.WriteClusterDefinition(configToUpdate.ClusterDefinition);
            sut.WriteClusterDefinition(configToUpdate.ClusterDefinition);
            sut.WriteClusterDefinition(configToUpdate.ClusterDefinition);
             
            // Assert
            configRepositoryMock.Verify(x => x.UpdateClusterConfig(It.IsAny<ClusterConfig>()), Times.Once());
        }
        
        private ClusterConfig GetSampleConfig()
        {
            return new ClusterConfig()
            {
                CurrentHost = new Host()
                {
                    Name = "Node1",
                    ListeningUrls = "http://localhost:5001;https://localhost:5002",
                    LocalAddress = "http://localhost:5001",
                    PublicAddress = "http://some-domaing:5002",
                    HostingModel = "InMemory",
                },
                ClusterDefinition = this.GetSampleClusterDefinition(),
            };
        }

        private ClusterDefinition GetSampleClusterDefinition()
        {
            return new ClusterDefinition()
            {
                HashMD5 = "123",
                LastUpdateTime = new DateTime(2013, 1, 1, 1, 1, 1),
                Hosts = this.GetSampleHosts(),
                ApplicationInstances = this.GetSampleApplicationInstances(),
            };
        }

        private Hosts GetSampleHosts()
        {
            return new Hosts()
            {
                new Host()
                {
                    Name = "Node1",
                    ListeningUrls = "http://localhost:5001;https://localhost:5002",
                    LocalAddress = "http://localhost:5001",
                    PublicAddress = "http://some-domaing:5002",
                    HostingModel = "InMemory",
                },
                new Host()
                {
                    Name = "Node2",
                    ListeningUrls = "http://localhost:5021;https://localhost:5022",
                    LocalAddress = "http://localhost:5021",
                    PublicAddress = "http://some-domaing:5021",
                    HostingModel = "InMemory",
                },
            };
        }

        private ApplicationInstances GetSampleApplicationInstances()
        {
            return new ApplicationInstances()
            {
                new ApplicationInstance()
                {
                    Name = "SampleAppInstanceA",
                    Type = "HelloWorld",
                    Count = "2",
                    HostingModel = "TestHostingModel",
                    PlacementPolicies = new List<string>()
                    {
                        "Node1",
                    },
                },
                new ApplicationInstance()
                {
                    Name = "SampleAppInstanceB",
                    Type = "HelloWorld",
                    Count = "3",
                    HostingModel = "TestHostingModel",
                    PlacementPolicies = new List<string>()
                    {
                        "Node2",
                    },
                },
            };
        }
    }
}