namespace DataGenies.Core.Models
{
    public class VirtualBinding
    {
        public string InstanceName { get; set; }

        public string TemplateName { get; set; }

        public string RoutingKey { get; set; }

        public VirtualBindingScope Scope { get; set; }
    }

    public enum VirtualBindingScope
    {
        Instance,
        Template
    }
}