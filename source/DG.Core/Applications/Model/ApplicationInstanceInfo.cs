namespace DG.Core.Orchestrators
{
    public struct ApplicationInfo
    {
        public ApplicationUniqueId ApplicationUniqueId { get; set; }

        public int InstanceCount { get; set; }

        public string PropertiesAsJson { get; set; }

        public string HostingModel { get; set; }
    }
}