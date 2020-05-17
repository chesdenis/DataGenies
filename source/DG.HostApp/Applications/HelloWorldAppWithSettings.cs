using System;
using DG.Core.Attributes;

namespace DG.HostApp.Applications
{
    [Application]
    public class HelloWorldAppWithSettings
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

        [Settings]
        public SampleSettings Settings { get; set; }
    }
    
    public class SampleSettings
    {
        [SharedSettings("SectionA:SharedA")]
        public string SharedA { get; set; }
        
        [SharedSettings("SectionB:SharedB")]
        public ComplexSharedB SharedB { get; set; }
        
        public string B { get; set; }
        
        public RegularComplexC RegularC { get; set; }
        
        public int F { get; set; }
    }

    public class ComplexSharedB
    {
        public string A { get; set; }

        public string B { get; set; }
    }
    
    public class RegularComplexC
    {
        public string A { get; set; }

        public string B { get; set; }
    }
}