using System;
using System.Collections.Generic;
using System.Configuration;
using DG.Core.ConfigManagers;
using DG.Core.Model.ClusterConfig;
using DG.Core.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace DG.Core.Tests.Unit
{
    public class ClusterConfigManagerTests
    {
        [Fact]
        public void InCaseOfJsonErrorShouldFailIfMissingCorrectClusterConfigCache()
        {
            // Arrange
            var configRepositoryMock = new Mock<IClusterConfigRepository>();
            configRepositoryMock.Setup(x => x.GetClusterConfig())
                .Throws(new Exception("Can't read or deserialize cluster config"));
            
            // Act
            var sut = new ClusterConfigManager(configRepositoryMock.Object);

            // Assert
            Assert.Throws<Exception>(() => sut.GetConfig());
        }
        
        [Fact]
        public void InCaseOfJsonErrorShouldUseClusterConfigCache()
        {
            // Arrange
            var configRepositoryMock = new Mock<IClusterConfigRepository>();
            var sampleConfig = this.GetSampleConfig();
            configRepositoryMock.Setup(x => x.GetClusterConfig())
                .Returns(sampleConfig);
            
            // Act
            var sut = new ClusterConfigManager(configRepositoryMock.Object);
            var config = sut.GetConfig();
            configRepositoryMock.Setup(x => x.GetClusterConfig())
                .Throws(new Exception("Can't read or deserialize cluster config"));
            config = sut.GetConfig();
            
            // Assert
            config.Should().NotBeNull();
        }

        [Fact]
        public void OnVerifyHashShouldReadConfigInformationFromRepository()
        {
            // Arrange
            var configRepositoryMock = new Mock<IClusterConfigRepository>();
            var sampleConfig = this.GetSampleConfig();
            configRepositoryMock.Setup(x => x.GetClusterConfig())
                .Returns(sampleConfig);
            
            // Act
            var sut = new ClusterConfigManager(configRepositoryMock.Object);
            sut.VerifyMd5Hash();

            // Assert
            configRepositoryMock.Verify(x=>x.GetClusterConfig(), Times.Exactly(1));
        }

        [Fact]
        public void ShouldReportVerifyMd5HashOnceWeHaveDifferentConfigs()
        {
            // Arrange
            var sampleConfigA = this.GetSampleConfig();
            var sampleConfigB = this.GetSampleConfig();
            var sampleConfigC = this.GetSampleConfig();

            sampleConfigA.Nodes[0].NodeName += " - renamed";
            sampleConfigB.Nodes.RemoveAt(0);
            sampleConfigC.Nodes.RemoveAt(0);
            
            var configRepositoryMockA = new Mock<IClusterConfigRepository>();
            var configRepositoryMockB = new Mock<IClusterConfigRepository>();
            var configRepositoryMockC = new Mock<IClusterConfigRepository>();
            
            configRepositoryMockA.Setup(x => x.GetClusterConfig()).Returns(sampleConfigA);
            configRepositoryMockB.Setup(x => x.GetClusterConfig()).Returns(sampleConfigB);
            configRepositoryMockC.Setup(x => x.GetClusterConfig()).Returns(sampleConfigC);
            
            // Act
            var configManagerOnNodeA = new ClusterConfigManager(configRepositoryMockA.Object);
            var configManagerOnNodeB = new ClusterConfigManager(configRepositoryMockB.Object);
            var configManagerOnNodeC = new ClusterConfigManager(configRepositoryMockC.Object);

            var hashOnNodeA = configManagerOnNodeA.GetMd5Hash();
            var hashOnNodeB = configManagerOnNodeB.GetMd5Hash();
            var hashOnNodeC = configManagerOnNodeC.GetMd5Hash();

            // put same hash for each config - for config A it should report fail for verify hash
            sampleConfigA.HashMD5 = hashOnNodeB;
            sampleConfigB.HashMD5 = hashOnNodeB;
            sampleConfigC.HashMD5 = hashOnNodeB;

            var configAVerifyResults = configManagerOnNodeA.VerifyMd5Hash();
            var configBVerifyResults = configManagerOnNodeB.VerifyMd5Hash();
            var configCVerifyResults = configManagerOnNodeC.VerifyMd5Hash();

            // Assert
            configAVerifyResults.Should().BeFalse();
            configBVerifyResults.Should().BeTrue();
            configCVerifyResults.Should().BeTrue();
            hashOnNodeA.Should().NotBe(hashOnNodeB);
            hashOnNodeC.Should().Be(hashOnNodeB);
        }

        [Fact]
        public void ShouldHashDifferentOnEachNodesUpdates()
        {
            // Arrange
            var configRepositoryMock = new Mock<IClusterConfigRepository>();
            var sampleConfig = this.GetSampleConfig();
            configRepositoryMock.Setup(x => x.GetClusterConfig()).Returns(sampleConfig);
            
            // Act
            var sut = new ClusterConfigManager(configRepositoryMock.Object);
            var md5HashA = sut.GetMd5Hash();
            
            sampleConfig.Nodes.RemoveAt(0);
            var md5HashRemovedInstance = sut.GetMd5Hash();

            sampleConfig.Nodes[0].HostName += " - renamed";
            var md5HashRenamedInstance = sut.GetMd5Hash();

            // Assert
            md5HashA.Should().NotBe(md5HashRemovedInstance);
            md5HashA.Should().NotBe(md5HashRenamedInstance);
            md5HashRemovedInstance.Should().NotBe(md5HashRenamedInstance);
        }
        
        [Fact]
        public void ShouldHashDifferentOnEachApplicationInstancesUpdates()
        {
            // Arrange
            var configRepositoryMock = new Mock<IClusterConfigRepository>();
            var sampleConfig = this.GetSampleConfig();
            configRepositoryMock.Setup(x => x.GetClusterConfig()).Returns(sampleConfig);
            
            // Act
            var sut = new ClusterConfigManager(configRepositoryMock.Object);
            var md5HashA = sut.GetMd5Hash();
            
            sampleConfig.ApplicationInstances.RemoveAt(0);
            var md5HashRemovedInstance = sut.GetMd5Hash();

            sampleConfig.ApplicationInstances[0].Name += " - renamed";
            var md5HashRenamedInstance = sut.GetMd5Hash();

            // Assert
            md5HashA.Should().NotBe(md5HashRemovedInstance);
            md5HashA.Should().NotBe(md5HashRenamedInstance);
            md5HashRemovedInstance.Should().NotBe(md5HashRenamedInstance);
        }
        
        [Fact]
        public void ShouldHashEqualIfApplicationInstancesHaveNoUpdates()
        {
            // Arrange
            var configRepositoryMock = new Mock<IClusterConfigRepository>();
            var sampleConfig = this.GetSampleConfig();
            configRepositoryMock.Setup(x => x.GetClusterConfig()).Returns(sampleConfig);
            
            // Act
            var sut = new ClusterConfigManager(configRepositoryMock.Object);
            var md5HashA = sut.GetMd5Hash();

            sampleConfig.ApplicationInstances[0].Name += " - renamed";
            var md5HashRenamedInstance = sut.GetMd5Hash();

            var restoredValue = sampleConfig.ApplicationInstances[0].Name.Replace(" - renamed", string.Empty);
            sampleConfig.ApplicationInstances[0].Name = restoredValue;
            var md5HashRestoredInstance = sut.GetMd5Hash();

            // Assert
            md5HashA.Should().Be(md5HashRestoredInstance);
            md5HashA.Should().NotBe(md5HashRenamedInstance);
        }
        
        [Fact]
        public void ShouldHashEqualIfNodesHaveNoUpdates()
        {
            // Arrange
            var configRepositoryMock = new Mock<IClusterConfigRepository>();
            var sampleConfig = this.GetSampleConfig();
            configRepositoryMock.Setup(x => x.GetClusterConfig()).Returns(sampleConfig);
            
            // Act
            var sut = new ClusterConfigManager(configRepositoryMock.Object);
            var md5HashA = sut.GetMd5Hash();

            sampleConfig.Nodes[0].NodeName += " - renamed";
            var md5HashRenamedInstance = sut.GetMd5Hash();

            var restoredValue = sampleConfig.Nodes[0].NodeName.Replace(" - renamed", string.Empty);
            sampleConfig.Nodes[0].NodeName = restoredValue;
            var md5HashRestoredInstance = sut.GetMd5Hash();

            // Assert
            md5HashA.Should().Be(md5HashRestoredInstance);
            md5HashA.Should().NotBe(md5HashRenamedInstance);
        }

        private ClusterConfig GetSampleConfig()
        {
            return new ClusterConfig()
            {
                Nodes = this.GetSampleNodes(),
                ApplicationInstances = this.GetSampleApplicationInstances(),
                HashMD5 = "123",
                LastUpdateTime = new DateTime(2013, 1, 1, 1, 1, 1),
            };
        }

        private Nodes GetSampleNodes()
        {
            return new Nodes()
            {
                new Node()
                {
                    NodeName = "Node1",
                    HostName = "localhost",
                    Port = 5001,
                    HostingModel = "InMemory",
                },
                new Node()
                {
                    NodeName = "Node2",
                    HostName = "localhost",
                    Port = 5021,
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