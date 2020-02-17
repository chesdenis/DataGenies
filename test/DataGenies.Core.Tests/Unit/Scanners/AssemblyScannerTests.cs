using System;
using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Models;
using DataGenies.Core.Scanners;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using DataGenies.Core.Tests.Integration.Mocks.Behaviours;
using DataGenies.Core.Tests.Integration.Mocks.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DataGenies.Core.Tests.Unit.Scanners
{
    [TestClass]
    public class AssemblyScannerTests
    {
        [TestMethod]
        public void ShouldScanApplicationTemplates()
        {
            // Arrange
            var sut =  Substitute.ForPartsOf<AssemblyScanner>(new DataGeniesOptions());
            sut.GetAssemblyTypes(Arg.Any<string>()).Returns(new List<Type>()
            {
                typeof(MockBrokenReceiver),
                typeof(MockPublisherMultipleMessagesDifferentRoutingKeys),
                typeof(MockSimplePublisher),
                typeof(MockSimpleReceiver),
                typeof(MockRevertTextAfterReceiveConverter),
                typeof(MockRevertTextBeforePublishConverter)
            });
            
            // Act
            var applicationTemplates = sut.ScanApplicationTemplates("path to asm");

            // Assert
            Assert.AreEqual(4, applicationTemplates.Count());
        }
        
        [TestMethod]
        public void ShouldScanBehaviours()
        {
            // Arrange
            var sut =  Substitute.ForPartsOf<AssemblyScanner>(new DataGeniesOptions());
            sut.GetAssemblyTypes(Arg.Any<string>()).Returns(new List<Type>()
            {
                typeof(MockBrokenReceiver),
                typeof(MockPublisherMultipleMessagesDifferentRoutingKeys),
                typeof(MockSimplePublisher),
                typeof(MockSimpleReceiver),
                typeof(MockBehaviour),
                typeof(MockRevertTextAfterReceiveConverter),
                typeof(MockRevertTextBeforePublishConverter)
            });
            
            // Act
            var applicationTemplates = sut.ScanBehaviours("path to asm");

            // Assert
            Assert.AreEqual(1, applicationTemplates.Count());
        }
    }
}