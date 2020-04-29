using System;
using System.Collections.Generic;
using System.Text;
using DG.Core.Orchestrators;
using DG.Core.Providers;
using DG.Core.Scanners;
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
            var instancesScanner = new Mock<IApplicationInstancesScanner>();
            var typeProvider = new Mock<ITypeProvider>();
            var instanceOrchestrator = new ApplicationInstanceOrchestrator(instancesScanner.Object, typeProvider.Object);
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
            var instancesScanner = new Mock<IApplicationInstancesScanner>();
            var typeProvider = new Mock<ITypeProvider>();
            var instanceOrchestrator = new ApplicationInstanceOrchestrator(instancesScanner.Object, typeProvider.Object);
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
            var instancesScanner = new Mock<IApplicationInstancesScanner>();
            var typeProvider = new Mock<ITypeProvider>();
            var instanceOrchestrator = new ApplicationInstanceOrchestrator(instancesScanner.Object, typeProvider.Object);
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
            var instancesScanner = new Mock<IApplicationInstancesScanner>();
            var typeProvider = new Mock<ITypeProvider>();
            var instanceOrchestrator = new ApplicationInstanceOrchestrator(instancesScanner.Object, typeProvider.Object);

            var instanceKey1 = "TestApp/SomeTestApp";
            var instanceKey2 = "SecondTestApp/SomeSecondTestApp";
            var instance1TypeName = "TestApp";
            var instance2TypeName = "SecondTestApp";
            var instancesKeysAndTypeNames = new Dictionary<string, string>()
            {
                { instanceKey1, instance1TypeName },
                { instanceKey2, instance2TypeName },
            };

            instancesScanner.Setup(x => x.Initialize());
            instancesScanner.Setup(x => x.GetInstancesNamesAndTypes()).Returns(instancesKeysAndTypeNames);
            typeProvider.Setup(x => x.GetInstanceType("TestApp")).Returns(typeof(TestApp));
            typeProvider.Setup(x => x.GetInstanceType("SecondTestApp")).Returns(typeof(SecondTestApp));

            // Act
            var preparedData = instanceOrchestrator.PrepareInstancesDataToCreate();

            // Assert
            preparedData.Should().Contain(new KeyValuePair<string, Type>(instanceKey1, typeof(TestApp)));
            preparedData.Should().Contain(new KeyValuePair<string, Type>(instanceKey2, typeof(SecondTestApp)));
        }

        [Fact]
        public void ShouldReturnInstancesInMemoryDictionary()
        {
            // Arrange
            var instancesScanner = new Mock<IApplicationInstancesScanner>();
            var typeProvider = new Mock<ITypeProvider>();
            var instanceOrchestrator = new ApplicationInstanceOrchestrator(instancesScanner.Object, typeProvider.Object);

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
