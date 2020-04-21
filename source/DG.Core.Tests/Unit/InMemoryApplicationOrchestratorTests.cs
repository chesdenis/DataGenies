using System.Linq;
using System.Xml.Schema;
using DG.Core.Model.Enums;
using DG.Core.Model.Output;
using DG.Core.Orchestrators;
using DG.Core.Tests.MockApplications;
using FluentAssertions;
using Xunit;

namespace DG.Core.Tests.Unit
{
    public class InMemoryApplicationOrchestratorTests  
    {
        [Fact]
        public void ShouldGetInstanceStateFromOrchestratorInCaseOfStringData()
        {
            // Arrange
            var inMemoryOrchestrator = new InMemoryApplicationOrchestrator();
            inMemoryOrchestrator.Register("testApp", typeof(SampleApplicationWithReportStateA), "instanceA");
            
            // Act
            var reports = inMemoryOrchestrator.GetInstanceState("testApp", "instanceA").ToList();
        
            // Assert
            reports.Should().HaveCount(1);
            reports.First().Should().Match<StateReport>(x => x.Status == Status.Finished);
        }
        
        [Fact]
        public void ShouldGetInstanceStateFromOrchestratorInCaseOfTypedData()
        {
            // Arrange
            var inMemoryOrchestrator = new InMemoryApplicationOrchestrator();
            inMemoryOrchestrator.Register("testApp", typeof(SampleApplicationWithReportStateB), "instanceA");
            
            // Act
            var reports = inMemoryOrchestrator.GetInstanceState("testApp", "instanceA").ToList();
        
            // Assert
            reports.Should().HaveCount(1);
            reports.First().Should().Match<StateReport>(x => x.Status == Status.Started);
        }
    }
}