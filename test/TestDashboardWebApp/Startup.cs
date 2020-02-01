using DataGenies.Core.Extensions;
using DataGenies.Core.Models.Contexts;
using DataGenies.UI.Extensions;
using DataGenies.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestDashboardWebApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataGeniesCoreServices();
            services.AddDataGeniesUIServices();

            services.AddSingleton<SchemaDataContext, SchemaDataContext>();
            services.AddSingleton<ISchemaDataContext>(provider => provider.GetService<SchemaDataContext>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseDataGeniesCore();
            app.UseDataGeniesUI();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
            });
            
             
        }
    }
}