using DG.Core.ConfigManagers;
using DG.Core.Model.ClusterConfig;
using DG.Core.Orchestrators;
using DG.Core.Providers;
using DG.Core.Repositories;
using DG.Core.Scanners;
using DG.Core.Services;
using DG.HostApp.Providers;
using DG.HostApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DG.HostApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddHostedService<ServiceWatcher>();
            services.AddControllers();

            services.AddTransient<IHttpService, HttpService>();
            services.AddTransient<ISystemClock, SystemClock>();
            
            services.AddTransient<IClusterConfigRepository, ClusterJsonConfigRepository>();
            services.AddSingleton<IClusterConfigManager, ClusterConfigManager>();
            services.AddSingleton<IApplicationOrchestrator, InMemoryApplicationOrchestrator>();
            services.AddTransient<IApplicationScanner, ApplicationScanner>();
            
            services.AddTransient<IClusterConfigProvider, ClusterConfigJsonProvider>();
            services.AddTransient<IFileSystemProvider, FileSystemProvider>();
            services.AddTransient<IAssemblyTypesProvider, AssemblyTypesProvider>();
            
            services.Configure<DG.Core.Model.ClusterConfig.Host>(this.Configuration.GetSection("CurrentHost"));
            services.Configure<DG.Core.Model.ClusterConfig.ApplicationTypesSources>(
                this.Configuration.GetSection("ClusterDefinition:ApplicationTypesSources"));
            services.Configure<DG.Core.Model.ClusterConfig.ApplicationInstances>(
                this.Configuration.GetSection("ClusterDefinition:ApplicationInstances"));

            services.AddSingleton(this.Configuration);
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

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}