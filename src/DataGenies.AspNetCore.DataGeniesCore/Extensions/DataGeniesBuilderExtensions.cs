using System;
using DataGenies.AspNetCore.DataGeniesCore.Middlewares;
using DataGenies.AspNetCore.DataGeniesCore.Middlewares.Responders;
using DataGenies.AspNetCore.DataGeniesCore.Models;
using DataGenies.AspNetCore.DataGeniesCore.Providers;
using DataGenies.AspNetCore.DataGeniesCore.Repositories;
using DataGenies.AspNetCore.DataGeniesCore.Scanners;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DataGenies.AspNetCore.DataGeniesCore.Extensions
{
    public static class DataGeniesBuilderExtensions
    {
        public static IApplicationBuilder UseDataGeniesCore(
            this IApplicationBuilder app,
            Action<DataGeniesOptions> setupAction = null)
        {
            var options = app.ApplicationServices.GetService<IOptions<DataGeniesOptions>>()?.Value ?? new DataGeniesOptions();
            setupAction?.Invoke(options);
            
            app.UseMiddleware<DataGeniesMiddleware>(options);

            return app;
        }

        public static void AddDataGeniesCoreServices(this IServiceCollection services)
        {
            services.AddTransient<IFileSystemRepository, FileSystemRepository>();
            services.AddTransient<IAssemblyTypesProvider, AssemblyTypesProvider>();
            services.AddTransient<IApplicationTypesScanner, ApplicationTypesScanner>();
            
            services.AddTransient<IDataGeniesMiddlewareResponder, GetApplicationTypesResponder>();

            services.AddSingleton<DataGeniesOptions>();
        }
    }
}