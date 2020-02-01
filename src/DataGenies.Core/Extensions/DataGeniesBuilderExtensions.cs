using System;
using DataGenies.Core.Middlewares;
using DataGenies.Core.Middlewares.Responders;
using DataGenies.Core.Models;
using DataGenies.Core.Repositories;
using DataGenies.Core.Scanners;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DataGenies.Core.Extensions
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
            services.AddTransient<IAssemblyScanner, AssemblyScanner>();
            services.AddTransient<IApplicationTemplatesScanner, ApplicationTemplatesScanner>();
            
            services.AddTransient<IDataGeniesMiddlewareResponder, ApplicationTypesResponder>();

            services.AddSingleton<DataGeniesOptions>();
        }
    }
}