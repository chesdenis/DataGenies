using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using DG.Core.Attributes;
using DG.Core.Model.Enums;
using DG.Core.Model.Output;
using DG.Core.Orchestrators;
using FluentAssertions;
using Xunit;

namespace DG.Core.Tests.Unit
{
    public class InMemoryApplicationOrchestratorTests  
    {
        [Theory]
        [InlineData("appName", "intanceNameWith//DoubleBackslashes")]
        [InlineData("appName", "intanceNameWith/Backslash")]
        [InlineData("appName", "intanceNameWith'Ampersand")]
        [InlineData("appName", "intanceNameWith\\Slash")]
        public void ShouldWorkWithApplicationsWithSpecializedNames(string applicationName, string instanceName)
        {
            // Arrange
            var inMemoryOrchestrator = new InMemoryApplicationOrchestrator();
            inMemoryOrchestrator.Register(applicationName, typeof(AppA), instanceName);
            
            // Act
            var reports = inMemoryOrchestrator.GetInstanceState(applicationName, instanceName).ToList();
        
            // Assert
            reports.Should().HaveCount(1);
            reports.First().Should().Match<StateReport>(x => x.Status == Status.Finished);
        }

        [Fact]
        public void ShouldGetInstanceStateInCaseOfStringDataInStateReport()
        {
            // Arrange
            var inMemoryOrchestrator = new InMemoryApplicationOrchestrator();
            inMemoryOrchestrator.Register("testApp", typeof(AppA), "instanceA");
            
            // Act
            var reports = inMemoryOrchestrator.GetInstanceState("testApp", "instanceA").ToList();
        
            // Assert
            reports.Should().HaveCount(1);
            reports.First().Should().Match<StateReport>(x => x.Status == Status.Finished);
        }
        
        [Fact]
        public void ShouldGetInstanceStateInCaseOfTypedDataInStateReport()
        {
            // Arrange
            var inMemoryOrchestrator = new InMemoryApplicationOrchestrator();
            inMemoryOrchestrator.Register("testApp", typeof(AppB), "instanceA");
            
            // Act
            var reports = inMemoryOrchestrator.GetInstanceState("testApp", "instanceA").ToList();
        
            // Assert
            reports.Should().HaveCount(1);
            reports.First().Should().Match<StateReport>(x => x.Status == Status.Started);
        }

        [Fact]
        public void ShouldThrowExceptionInGetInstanceStateInCaseOfMissingStateReport()
        {
            // Arrange
            var inMemoryOrchestrator = new InMemoryApplicationOrchestrator();
            inMemoryOrchestrator.Register("testAppC", typeof(AppC), "instanceA");
            inMemoryOrchestrator.Register("testAppD", typeof(AppD), "instanceA");
            
            // Act
            var reportsC = Assert.Throws<ArgumentOutOfRangeException>(() =>
                inMemoryOrchestrator.GetInstanceState("testAppC", "instanceA").ToList());

            var reportsD = Assert.Throws<ArgumentOutOfRangeException>(() =>
                inMemoryOrchestrator.GetInstanceState("testAppD", "instanceA").ToList());

            // Assert
            reportsC.Should().BeOfType<ArgumentOutOfRangeException>();
            reportsD.Should().BeOfType<ArgumentOutOfRangeException>();
        }
        
        [Fact]
        public void ShouldThrowExceptionInGetInstanceStateInCaseOfMissingApplications()
        {
            // Arrange
            var inMemoryOrchestrator = new InMemoryApplicationOrchestrator();
            
            // Act
            var reportsC = Assert.Throws<KeyNotFoundException>(() =>
                inMemoryOrchestrator.GetInstanceState("testAppC", "instanceA").ToList());
            
            // Assert
            reportsC.Should().BeOfType<KeyNotFoundException>();
        }
    }
    
    [Application]
    internal class AppA
    {
        [StateReport]
        public string ReportStateSampleFunction()
        {
            return "{'Status':'0'}";
        }
    }
    
    [Application]
    internal class AppB
    {
        [StateReport]
        public StateReport ReportStateSampleFunction()
        {
            return new StateReport()
            {
                Status = Status.Started,
            };
        }
    }
    
    [Application]
    internal class AppC
    {
    }
    
    internal class AppD
    {
    }
}