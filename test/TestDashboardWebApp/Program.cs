using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DataGenies.Core.Models;
using DataGenies.InMemory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TestDashboardWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildData();
            
            CreateHostBuilder(args).Build().Run();
        }

        private static void BuildData()
        {
            var inMemorySchemaContext = new SchemaDataContext();

            var sampleAppTemplate = new ApplicationTemplate
            {
                Id = 1,
                Name = "SampleAppTemplate",
                Version = "2019.1.1",
                AssemblyPath = string.Empty,
                ConfigTemplateJson = "{}"
            };
            var sampleAppInstance = new ApplicationInstance
            {
                Id = 1,
                Name = "SampleAppInstance",
                ConfigJson = "{}",
                IncomingBindings = new List<Binding>(),
                OutcomingBindings = new List<Binding>()
            };
            sampleAppInstance.Template = sampleAppTemplate;
            sampleAppInstance.TemplateId = sampleAppTemplate.Id;

            inMemorySchemaContext.ApplicationTemplates.Add(sampleAppTemplate);
            inMemorySchemaContext.ApplicationInstances.Add(sampleAppInstance);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}