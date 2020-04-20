namespace DG.HostApp.Applications
{
    using System;
    using DG.Core.Attributes;

    [Application]
    public class HelloWorldApp
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
    }
}