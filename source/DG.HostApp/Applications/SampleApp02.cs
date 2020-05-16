﻿using System;
using DG.Core.Attributes;

namespace DG.HostApp.Applications
{
    [Application]
    public class SampleApp02
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