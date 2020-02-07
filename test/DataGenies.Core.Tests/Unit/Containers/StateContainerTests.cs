using System;
using DataGenies.Core.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataGenies.Core.Tests.Unit.Containers
{
    [TestClass]
    public class StateContainerTests
    {
        private interface IMockInterface
        {
            void TestMethod(string testValue);
        }

        private class MockClass : IMockInterface
        {
            public string TestValue { get; set; }

            public void TestMethod(string testValue)
            {
                this.TestValue = testValue;
            }
        }


        [TestMethod]
        public void ShouldStoreAndGetContainerByType()
        {
            //Arrange
            var sut = new Container();
            var mockInstance = new MockClass();
            
            //Act
            sut.Register<IMockInterface>(mockInstance);
            sut.Resolve<IMockInterface>().TestMethod("1234");
            
            //Assert
            Assert.AreEqual("1234", mockInstance.TestValue);
        }

        [TestMethod]
        public void ShouldStoreAndGetContainerByTypeAndName()
        {
            //Arrange
            var sut = new Container();
            var mockInstanceA = new MockClass();
            var mockInstanceB = new MockClass();
            
            //Act
            sut.Register<IMockInterface>(mockInstanceA, "A");
            sut.Register<IMockInterface>(mockInstanceB, "B");
            sut.Resolve<IMockInterface>("A").TestMethod("A123");
            sut.Resolve<IMockInterface>("B").TestMethod("B123");
            
            //Assert
            Assert.AreEqual("A123", mockInstanceA.TestValue);
            Assert.AreEqual("B123", mockInstanceB.TestValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionIfTypeExistsAndNoNameProvided()
        {
            //Arrange
            var sut = new Container();
            var mockInstance = new MockClass();
            
            //Act
            sut.Register<IMockInterface>(mockInstance);
            sut.Register<IMockInterface>(mockInstance);
            
            sut.Resolve<IMockInterface>().TestMethod("1234");
            
            //Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionIfTypeWithNameExists()
        {
            //Arrange
            var sut = new Container();
            var mockInstanceA = new MockClass();
             
            //Act
            sut.Register<IMockInterface>(mockInstanceA, "A");
            sut.Register<IMockInterface>(mockInstanceA, "A");
            sut.Resolve<IMockInterface>("A").TestMethod("A123");
            
            //Assert
            Assert.Fail();
        }
    }
}