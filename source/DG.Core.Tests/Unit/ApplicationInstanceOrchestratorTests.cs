using System;
using System.Collections.Generic;
using DG.Core.Model.ClusterConfig;
using DG.Core.Orchestrators;
using DG.Core.Providers;
using FluentAssertions;
using Moq;
using Xunit;

namespace DG.Core.Tests.Unit
{
    public class ApplicationInstanceOrchestratorTests
    {
        [Fact]
        public void ShouldCreateApplicationInstance()
        {
            // Arrange
            var typeProvider = new Mock<ITypeProvider>();
            var applicationInstances = new List<ApplicationInstance>()
            {
                {
                    new ApplicationInstance()
                     {
                        Count = "1",
                        HostingModel = "InMemory",
                        Type = "TestApp",
                        Name = "SomeTestApp",
                        PlacementPolicies = new List<string>() { "Node1" },
                     }
                },
            };

            var instanceOrchestrator = new ApplicationInstanceOrchestrator(typeProvider.Object);
            var instanceKey = "TestApp/SomeTestApp";
            var instanceType = typeof(TestApp);

            // Act
            instanceOrchestrator.CreateSingleInstanceInMemory(instanceKey, instanceType);
            var inMemoryInstances = instanceOrchestrator.GetInMemoryInstancesData();
            var createdInstance = inMemoryInstances[instanceKey];

            // Assert
            inMemoryInstances.Contains(new KeyValuePair<string, object>(instanceKey, createdInstance));
            createdInstance.Should().BeOfType(instanceType);
        }

        [Fact]
        public void ShouldThrowExceptionIfCreated()
        {
            // Arrange
            var typeProvider = new Mock<ITypeProvider>();
            var applicationInstances = new List<ApplicationInstance>()
            {
                {
                    new ApplicationInstance()
                     {
                        Count = "1",
                        HostingModel = "InMemory",
                        Type = "TestApp",
                        Name = "SomeTestApp",
                        PlacementPolicies = new List<string>() { "Node1" },
                     }
                },
            };
            var instanceOrchestrator = new ApplicationInstanceOrchestrator(typeProvider.Object);
            var instanceKey = "TestApp/SomeTestApp";
            var instanceType = typeof(TestApp);

            // Act
            instanceOrchestrator.CreateSingleInstanceInMemory(instanceKey, instanceType);
            Assert.Throws<ArgumentException>(() =>
            instanceOrchestrator.CreateSingleInstanceInMemory(instanceKey, instanceType));
        }

        [Fact]
        public void ShouldCreateApplicationInstances()
        {
            // Arrange
            var typeProvider = new Mock<ITypeProvider>();
            var applicationInstances = new List<ApplicationInstance>()
            {
                {
                    new ApplicationInstance()
                     {
                        Count = "1",
                        HostingModel = "InMemory",
                        Type = "TestApp",
                        Name = "SomeTestApp",
                        PlacementPolicies = new List<string>() { "Node1" },
                     }
                },
            };
            var instanceOrchestrator = new ApplicationInstanceOrchestrator(typeProvider.Object);
            var instanceKey1 = "TestApp/SomeTestApp";
            var instanceKey2 = "SecondTestApp/SomeSecondTestApp";
            var instancesToCreate = new Dictionary<string, Type>()
            {
                { instanceKey1, typeof(TestApp) },
                { instanceKey2, typeof(SecondTestApp) },
            };

            // Act
            instanceOrchestrator.CreateInstancesInMemory(instancesToCreate);
            var inMemoryInstances = instanceOrchestrator.GetInMemoryInstancesData();
            var testAppInstance = inMemoryInstances[instanceKey1];
            var secondTestAppInstance = inMemoryInstances[instanceKey2];

            // Assert
            inMemoryInstances.Should().Contain(new KeyValuePair<string, object>(instanceKey1, testAppInstance));
            inMemoryInstances.Should().Contain(new KeyValuePair<string, object>(instanceKey2, secondTestAppInstance));
            testAppInstance.Should().BeOfType(typeof(TestApp));
            secondTestAppInstance.Should().BeOfType(typeof(SecondTestApp));
        }

        [Fact]
        public void ShouldPrepareInstanceDataToBeCreated()
        {
            // Arrange
            var typeProvider = new Mock<ITypeProvider>();
            var applicationInstances = new List<ApplicationInstance>()
            {
                {
                    new ApplicationInstance()
                     {
                        Count = "1",
                        HostingModel = "InMemory",
                        Type = "TestApp",
                        Name = "SomeTestApp",
                        PlacementPolicies = new List<string>() { "Node1" },
                     }
                },
                {
                    new ApplicationInstance()
                     {
                        Count = "1",
                        HostingModel = "InMemory",
                        Type = "SecondTestApp",
                        Name = "SomeSecondTestApp",
                        PlacementPolicies = new List<string>() { "Node1" },
                     }
                },
            };
            var instanceOrchestrator = new ApplicationInstanceOrchestrator(typeProvider.Object);
            typeProvider.Setup(x => x.GetInstanceType("TestApp")).Returns(typeof(TestApp));
            typeProvider.Setup(x => x.GetInstanceType("SecondTestApp")).Returns(typeof(SecondTestApp));

            // Act
            var preparedData = instanceOrchestrator.PrepareInstancesDataToCreate(applicationInstances);

            // Assert
            preparedData.Should().Contain(new KeyValuePair<string, Type>(
                applicationInstances[0].Type + "/" + applicationInstances[0].Name, typeof(TestApp)));
            preparedData.Should().Contain(new KeyValuePair<string, Type>(
               applicationInstances[1].Type + "/" + applicationInstances[1].Name, typeof(SecondTestApp)));
        }

        [Fact]
        public void ShouldReturnInstancesInMemoryDictionary()
        {
            // Arrange            
            var typeProvider = new Mock<ITypeProvider>();
            var applicationInstances = new List<ApplicationInstance>()
            {
                {
                    new ApplicationInstance()
                     {
                        Count = "1",
                        HostingModel = "InMemory",
                        Type = "TestApp",
                        Name = "SomeTestApp",
                        PlacementPolicies = new List<string>() { "Node1" },
                     }
                },
                {
                    new ApplicationInstance()
                     {
                        Count = "1",
                        HostingModel = "InMemory",
                        Type = "SecondTestApp",
                        Name = "SomeSecondTestApp",
                        PlacementPolicies = new List<string>() { "Node1" },
                     }
                },
            };
            var instanceOrchestrator = new ApplicationInstanceOrchestrator(typeProvider.Object);

            // Act
            var instancesInMemory = instanceOrchestrator.GetInMemoryInstancesData();

            // Assert
            instancesInMemory.Should().NotBeNull();
        }
    }

    public class TestApp
    {
    }

    public class SecondTestApp
    {
    }
}
