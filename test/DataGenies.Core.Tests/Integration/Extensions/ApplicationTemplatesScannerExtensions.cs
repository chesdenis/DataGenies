using System;
using System.Collections;
using DataGenies.Core.Models;
using DataGenies.Core.Scanners;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using NSubstitute;

namespace DataGenies.Core.Tests.Integration.Extensions
{
    public static class ApplicationTemplatesScannerExtensions
    {
        public static void RegisterMockApplicationTemplate(
            this IApplicationTemplatesScanner applicationTemplatesScanner,
            Type applicationTemplateType,
            string applicationTemplateName)
        {
            applicationTemplatesScanner.FindType(Arg.Is<ApplicationTemplateEntity>
                    (w => w.Name == applicationTemplateName))
                            .Returns(applicationTemplateType);
        }
    }
}