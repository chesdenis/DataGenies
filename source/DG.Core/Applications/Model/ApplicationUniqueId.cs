using DG.Core.Extensions;

namespace DG.Core.Orchestrators
{
    public struct ApplicationUniqueId
    {
        public string Application { get; set; }

        public string InstanceName { get; set; }

        public override string ToString()
        {
            return ApplicationExtensions.ConstructUniqueId(this.Application, this.InstanceName);
        }
    }
}