namespace DG.HostApp.Extensions
{
    using DG.HostApp.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class ClusterConfigExtensions
    {
        public static IServiceCollection AddClusterConfigService(this IServiceCollection services)
        {
            return services.AddSingleton<ClusterConfigService>();
        }
    }
}