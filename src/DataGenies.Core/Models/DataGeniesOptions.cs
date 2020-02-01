using System;
using System.IO;
using System.Reflection;

namespace DataGenies.Core.Models
{
    public class DataGeniesOptions
    {
        public string RoutePrefix { get; set; } = "data-genies";

        public string DocumentTitle { get; set; } = "DATA Genies";

        public DropFolderOptions DropFolderOptions { get; set; } = new DropFolderOptions()
        {
            Path = "C:\\GeniesDropFolder",
            UseZippedPackages = false
        };
    }
}