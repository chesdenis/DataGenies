using System.Runtime.CompilerServices;
using DG.Core.Extensions;
using DG.Core.Model.ClusterConfig;
using DG.Core.Repositories;

namespace DG.HostApp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(x =>
                {
                    x.AddJsonFile("dg-cluster.config.json", false, true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    
                    webBuilder.UseUrls(GetClusterConfig().CurrentHost.GetHostListeningAddress());
                });

        public static ClusterConfig GetClusterConfig() => File.ReadAllText(ClusterJsonConfigRepository.ClusterConfigPath).FromJson<ClusterConfig>();
    }
}