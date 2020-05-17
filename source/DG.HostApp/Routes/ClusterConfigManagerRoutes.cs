using System;
using DG.Core.Model.ClusterConfig;

namespace DG.HostApp.Routes
{
    public static class HostRoutesExtensions
    {
        public static string BuildPublicEndpoint<T>(this Host host, string route)
            where T : DgRoutes, new()
        {
            return BuildEndpoint<T>(host, IpAddressType.Public, route);
        }
        
        public static string BuildLocalEndpoint<T>(this Host host, string route)
            where T : DgRoutes, new()
        {
            return BuildEndpoint<T>(host, IpAddressType.Local, route);
        }
         
        public static string BuildEndpoint<T>(this Host host, IpAddressType addressType, string route)
            where T : DgRoutes, new()
        {
            var dgRoutes = new T();
            
            var apiRoute = addressType switch
            {
                IpAddressType.Public => $"{host.LocalAddress}/api",
                IpAddressType.Local => $"{host.PublicAddress}/api",
                _ => throw new ArgumentOutOfRangeException(nameof(addressType))
            };

            return $"{apiRoute}/{dgRoutes.Root()}/{route}";
        }
    }

    public enum IpAddressType
    {
        Public,
        Local,
    }
     
    public static class Test
    {
        public static void Test2()
        {
            var host = new Host();

            var endpoint =
                host.BuildLocalEndpoint<ClusterConfigManagerRoutes>(ClusterConfigManagerRoutes.WriteConfig);

            // ClusterConfigManagerRoute.BuildEndpoint(ClusterConfigManagerRoute.WriteConfig);
            //
            // DgRoute<ClusterConfigManagerRoute>.For(new Host(), RouteType.Local).Build(ClusterConfigManagerRoute.WriteConfig);
        }

    }

    // public static class DgRouteTest<T>
    //     where T : DgBaseRoute, new()
    // {
    //     public static T For(Host host, RouteType routeType)
    //     {
    //         return new T
    //         {
    //             Host = host,
    //             RouteType = routeType,
    //         };
    //     }
    // }
    //
    // public class DgRoute<T>
    //     where T : DgBaseRoute, new()
    // {
    //     public static T Use(Host host, RouteType routeType)
    //     {
    //         return new T
    //         {
    //             Host = host,
    //             RouteType = routeType,
    //         };
    //     }
    // }

    public abstract class DgRoutes
    {
        public abstract string Root();
    }
    
    public class ClusterConfigManagerRoutes : DgRoutes
    {
        public override string Root() => "ClusterConfigManager";
        
        public const string WriteConfig = "WriteConfig";
    }
}