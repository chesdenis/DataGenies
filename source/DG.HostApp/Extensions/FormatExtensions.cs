using DG.Core.Model.ClusterConfig;

namespace DG.HostApp.Extensions
{
    public static class FormatExtensions
    {
        public static string RenderNameAndType(this ApplicationInstance applicationInstance)
        {
            return $"{applicationInstance.Name} ({applicationInstance.Type})";
        }
    }
}