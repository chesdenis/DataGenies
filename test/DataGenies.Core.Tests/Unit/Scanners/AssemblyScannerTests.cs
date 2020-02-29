using System;
using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Behaviours.BuiltIn;
using DataGenies.Core.Models;
using DataGenies.Core.Scanners;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
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
            var sut =  Substitute.ForPartsOf<AssemblyScanner>();
            sut.GetAssemblyTypes(Arg.Any<string>()).Returns(new List<Type>()
            {
                typeof(AssemblyScanner),
                typeof(MockSimplePublisher),
                typeof(MockSimpleReceiver)
            });
            
            // Act
            var applicationTemplates = sut.ScanApplicationTemplates("path to asm");

            // Assert
            Assert.AreEqual(2, applicationTemplates.Count());
        }
        
        [TestMethod]
        public void ShouldScanBehaviours()
        {
            // Arrange
            var sut =  Substitute.ForPartsOf<AssemblyScanner>();
            sut.GetAssemblyTypes(Arg.Any<string>()).Returns(new List<Type>()
            {
                typeof(AssemblyScanner),
                typeof(CompressionBehaviourTemplate),
                typeof(DecompressionBehaviourTemplate)
            });
            
            // Act
            var applicationTemplates = sut.ScanBehaviourTemplates("path to asm");

            // Assert
            Assert.AreEqual(2, applicationTemplates.Count());
        }
    }
}