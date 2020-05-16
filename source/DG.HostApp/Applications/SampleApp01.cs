using System;
using DG.Core.Attributes;

namespace DG.HostApp.Applications
{
    [Application]
    public class HelloWorldAppWithProperties
    {
        [Start]
        public void OnStart()
        {
            Console.WriteLine("Hello World!");
        }

        [Stop]
        public void OnStop()
        {
            Console.WriteLine("Hello World!");
        }

        [Properties]
        public SettingsA Settings { get; set; }
    }
    
    public class SettingsA
    {
        [SharedProperties("SectionA:SharedPropertyA")]
        public string SimpleSharedPropertyA { get; set; }
        
        [SharedProperties("SectionB:SharedPropertiesB")]
        public ComplexSharedPropertyB SubPropertiesA { get; set; }
        
        public string SimplePropertyB { get; set; }
        
        public RegularComplexPropertyC ComplexPropertyC { get; set; }
        
        public int SimpleNumericPropertyF { get; set; }
    }

    public class ComplexSharedPropertyB
    {
        public string SampleSubPropertyA { get; set; }

        public string SampleSubPropertyB { get; set; }
    }
    
    public class RegularComplexPropertyC
    {
        public string SampleSubPropertyA { get; set; }

        public string SampleSubPropertyB { get; set; }
    }
}