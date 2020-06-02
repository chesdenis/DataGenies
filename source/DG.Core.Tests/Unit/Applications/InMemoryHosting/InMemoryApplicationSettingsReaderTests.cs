using DG.Core.Applications.InMemoryHosting;
using DG.Core.Attributes;
using DG.Core.Extensions;
using DG.Core.Orchestrators;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DG.Core.Tests.Unit.Applications.InMemoryHosting
{
    public class InMemoryApplicationSettingsReaderTests
    {
        [Fact]
        public void ShouldReadApplicationSettingsIfNeeded()
        {
            // Arrange
            var writer = new InMemoryApplicationSettingsWriter();
            var inMemoryApplications = new InMemoryApplications();
            var app = new AppE();
            var appUniqueId = ApplicationExtensions.ParseUniqueId("sampleType/InstanceA");

            this.RegisterApplicationInstance(inMemoryApplications, appUniqueId, app);
            writer.WriteSettings(app, inMemoryApplications[appUniqueId].Metadata.PropertiesAsJson);

            var sut = new InMemoryApplicationSettingsReader(inMemoryApplications);

            // Act
            var settings = sut.GetSettings(appUniqueId);

            // Assert
            settings.Should().BeOfType(typeof(SampleSettings));
            settings.As<SampleSettings>().B.Should().Be("This is simple property B");
            settings.As<SampleSettings>().F.Should().Be(1234);
            settings.As<SampleSettings>().RegularC.A.Should().Be("This is regular sub property A");
            settings.As<SampleSettings>().RegularC.B.Should().Be("This is regular sub property B");
        }

        [Fact]
        public void ShouldReadApplicationSharedSettingsIfNeeded()
        {
            // Arrange
            var writer = new InMemoryApplicationSettingsWriter();
            var inMemoryApplications = new InMemoryApplications();
            var app = new AppE();
            var appUniqueId = ApplicationExtensions.ParseUniqueId("sampleType/InstanceA");

            this.RegisterApplicationInstance(inMemoryApplications, appUniqueId, app);
            writer.WriteSettings(app, inMemoryApplications[appUniqueId].Metadata.PropertiesAsJson);

            var sut = new InMemoryApplicationSettingsReader(inMemoryApplications);

            // Act
            var sharedSettings = sut.GetSharedSettings(appUniqueId);

            // Assert
            sharedSettings.Should().HaveCount(2);
            sharedSettings.ElementAt(0).Should().BeOfType(typeof(string));
            sharedSettings.ElementAt(1).Should().BeOfType(typeof(ComplexSharedB));

            sharedSettings.ElementAt(0).As<string>().Should().Be("This is shared A");

            sharedSettings.ElementAt(1).As<ComplexSharedB>().A.Should().Be("This is sub shared A");
            sharedSettings.ElementAt(1).As<ComplexSharedB>().B.Should().Be("This is sub shared B");
        }

        private void RegisterApplicationInstance(InMemoryApplications inMemoryApplications, ApplicationUniqueId appUniqueId, object applicationInstance)
        {
            inMemoryApplications.Add(
               appUniqueId,
               new InMemoryApplication()
               {
                   Metadata = new ApplicationInfo()
                   {
                       ApplicationUniqueId = appUniqueId,
                       HostingModel = "InMemory",
                       InstanceCount = 1,
                       PropertiesAsJson = this.GetExampleSettingsAsJson(),
                   },
                   Instances = new List<object>() { applicationInstance },
               });
        }

        private string GetExampleSettingsAsJson()
        {
            return @"{
        ""SharedSettings"": {
            ""SectionA"": {
                ""SharedA"": ""This is shared A""
            },
            ""SectionB"": {
                ""SharedB"": {
                    ""A"": ""This is sub shared A"",
                    ""B"": ""This is sub shared B""
                }
            }
        },
        ""Settings"": {
            ""B"": ""This is simple property B"",
            ""RegularC"": {
                ""A"": ""This is regular sub property A"",
                ""B"": ""This is regular sub property B""
            },
            ""F"": 1234
        }
}";
        }

        [Application]
        internal class AppE
        {
            [SharedSettings("SectionA:SharedA")]
            public string SharedA { get; set; }

            [SharedSettings("SectionB:SharedB")]
            public ComplexSharedB SharedB { get; set; }

            [Settings]
            public SampleSettings Settings { get; set; }
        }

        internal class SampleSettings
        {
            public string B { get; set; }

            public RegularComplexC RegularC { get; set; }

            public int F { get; set; }
        }

        internal class ComplexSharedB
        {
            public string A { get; set; }

            public string B { get; set; }
        }

        internal class RegularComplexC
        {
            public string A { get; set; }

            public string B { get; set; }
        }
    }
}