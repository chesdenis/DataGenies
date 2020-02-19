using System;
using DataGenies.Core.Models;
using DataGenies.Core.Scanners;
using NSubstitute;

namespace DataGenies.Core.Tests.Integration.Extensions
{
    public static class BehaviourTemplatesScannerExtensions
    {
        public static void RegisterMockBehaviourTemplate(
            this IBehaviourTemplatesScanner behaviourTemplatesScanner,
            Type behaviourTemplateType,
            string behaviourTemplateName)
        {
            behaviourTemplatesScanner.FindType(Arg.Is<BehaviourTemplateEntity>
                    (w => w.Name == behaviourTemplateName))
                .Returns(behaviourTemplateType);
        }
    }
}