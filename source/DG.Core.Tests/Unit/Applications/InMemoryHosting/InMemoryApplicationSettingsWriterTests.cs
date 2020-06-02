﻿using DG.Core.Applications.InMemoryHosting;
using DG.Core.Attributes;
using FluentAssertions;
using Xunit;

namespace DG.Core.Tests.Unit.Applications.InMemoryHosting
{

    public class InMemoryApplicationSettingsWriterTests
    {
        [Fact]
        public void ShouldWriteApplicationSettingsIfNeeded()
        {
            // Arrange
            var sut = new InMemoryApplicationSettingsWriter();
            var app = new AppE();

            // Act
            sut.WriteSettings(app, this.GetExampleSettingsAsJson());

            // Assert
            app.Settings.B.Should().Be("This is simple property B");
            app.Settings.F.Should().Be(1234);
            app.Settings.RegularC.A.Should().Be("This is regular sub property A");
            app.Settings.RegularC.B.Should().Be("This is regular sub property B");
        }

        [Fact]
        public void ShouldWriteApplicationSharedSettingsIfNeeded()
        {
            // Arrange
            var sut = new InMemoryApplicationSettingsWriter();
            var app = new AppE();

            // Act
            sut.WriteSettings(app, this.GetExampleSettingsAsJson());

            // Assert
            app.SharedA.Should().Be("This is shared A");

            app.SharedB.A.Should().Be("This is sub shared A");
            app.SharedB.B.Should().Be("This is sub shared B");
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