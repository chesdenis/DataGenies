using System;
using System.IO;
using System.Reflection;

namespace DataGenies.AspNetCore.DataGeniesCore
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
       
        public Func<Stream> IndexStream { get; set; } = () => typeof(DataGeniesOptions).GetTypeInfo().Assembly
            .GetManifestResourceStream("DataGenies.AspNetCore.DataGeniesCore.index.html");
    }
}