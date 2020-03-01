using DataGenies.Core.Configurators;
using DataGenies.Core.InMemory;
using DataGenies.Core.InMemory.Messaging;
using DataGenies.Core.Models;
using DataGenies.Core.Orchestrators;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Repositories;
using DataGenies.Core.Scanners;
using DataGenies.Core.Services;
using DataGenies.InMemoryHost.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DataGenies.InMemoryHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            
            services.AddDbContext<IFlowSchemaContext, FlowSchemaDbContext>(
                optionsBuilder =>
                    optionsBuilder.UseSqlServer(
                        this.Configuration.GetSection("DataGeniesConfig")
                            .GetValue<string>("SqlServerConnectionString")));

            services.AddScoped<ManagedServiceBuilder>();
            services.AddSingleton<InMemoryMqBroker>();

            services.AddScoped<IMqConfigurator, InMemoryMqConfigurator>();
            services.AddScoped<IBindingConfigurator, BindingConfigurator>();
            
            services.AddSingleton<IReceiverBuilder, InMemoryReceiverBuilder>();
            services.AddSingleton<IPublisherBuilder, InMemoryPublisherBuilder>();
            
            services.AddSingleton<IOrchestrator, InMemoryOrchestrator>();
            services.AddSingleton<IApplicationTemplatesScanner, ApplicationTemplatesScanner>();
            services.AddSingleton<IBehaviourTemplatesScanner, BehavioursTemplatesScanner>();
            
            services.AddScoped<IFileSystemRepository, FileSystemRepository>();
            services.AddScoped<IAssemblyScanner, AssemblyScanner>();

          

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller}/{action=Index}/{id?}");
                });

            app.UseSpa(
                spa =>
                {
                    // To learn more about options for serving an Angular SPA from ASP.NET Core,
                    // see https://go.microsoft.com/fwlink/?linkid=864501

                    spa.Options.SourcePath = "ClientApp";

                    if (env.IsDevelopment())
                    {
                        spa.UseAngularCliServer(npmScript: "start");
                    }
                });
        }
    }
}