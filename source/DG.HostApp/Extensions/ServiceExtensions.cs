namespace DG.HostApp.Extensions
{
    using DG.Core.ConfigManagers;
    using DG.HostApp.Services.PageServices;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IClusterConfigPageService, ClusterConfigPageService>();
            return services.AddSingleton<IOrchestratorPageService, OrchestratorPageService>();
        }
    }
}