using System;
using DataGenies.AspNetCore.DataGeniesCore.Middlewares;
using DataGenies.AspNetCore.DataGeniesCore.Middlewares.Responders;
using DataGenies.AspNetCore.DataGeniesCore.Models;
using DataGenies.AspNetCore.DataGeniesCore.Providers;
using DataGenies.AspNetCore.DataGeniesCore.Repositories;
using DataGenies.AspNetCore.DataGeniesCore.Scanners;
using DataGenies.AspNetCore.DataGeniesUI.Middlewares;
using DataGenies.AspNetCore.DataGeniesUI.Middlewares.Responders;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DataGenies.AspNetCore.DataGeniesUI.Extensions
{
    public static class DataGeniesUiExtensions
    {
        public static IApplicationBuilder UseDataGeniesUI(
            this IApplicationBuilder app,
            Action<DataGeniesOptions> setupAction = null)
        {
            var options = app.ApplicationServices.GetService<IOptions<DataGeniesOptions>>()?.Value ?? new DataGeniesOptions();
            setupAction?.Invoke(options);
            
            app.UseMiddleware<DataGeniesUIMiddleware>(options);

            return app;
        }
        
        public static void AddDataGeniesUIServices(this IServiceCollection services)
        {
            services.AddTransient<IFileSystemRepository, FileSystemRepository>();
            services.AddTransient<IAssemblyTypesProvider, AssemblyTypesProvider>();
            
            services.AddTransient<IDataGeniesMiddlewareResponder, IndexResponder>();
            
            services.AddTransient<DataGeniesOptions>();
        }
    }
}