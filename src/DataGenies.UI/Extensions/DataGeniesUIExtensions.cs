using System;
using DataGenies.AspNetCore.DataGeniesCore.Middlewares.Responders;
using DataGenies.Core.Middlewares;
using DataGenies.Core.Middlewares.Responders;
using DataGenies.Core.Models;
using DataGenies.Core.Repositories;
using DataGenies.Core.Scanners;
using DataGenies.UI.Middlewares;
using DataGenies.UI.Middlewares.Responders;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DataGenies.UI.Extensions
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
            services.AddTransient<IAssemblyScanner, AssemblyScanner>();

            services.AddTransient<IDataGeniesMiddlewareResponder, RedirectResponder>();
            services.AddTransient<IDataGeniesMiddlewareResponder, IndexResponder>();
        }
    }
}