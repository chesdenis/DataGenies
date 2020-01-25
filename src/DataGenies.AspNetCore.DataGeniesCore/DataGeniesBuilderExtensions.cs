using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DataGenies.AspNetCore.DataGeniesCore
{
    public static class DataGeniesBuilderExtensions
    {
        public static IApplicationBuilder UseDataGenies(
            this IApplicationBuilder app,
            Action<DataGeniesOptions> setupAction = null)
        {
            var options = app.ApplicationServices.GetService<IOptions<DataGeniesOptions>>()?.Value ?? new DataGeniesOptions();
            setupAction?.Invoke(options);
            
            app.UseMiddleware<DataGeniesMiddleware>(options);

            return app;
        }
    }
}