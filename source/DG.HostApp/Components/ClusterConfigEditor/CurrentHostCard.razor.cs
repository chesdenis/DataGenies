using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Core.Model.ClusterConfig;
using Microsoft.AspNetCore.Components;

namespace DG.HostApp.Components.ClusterConfigEditor
{
    public partial class CurrentHostCard
    {
        private Host selectedHost;

        [Parameter]
        public ClusterConfig ClusterConfig { get; set; }

        private void SetHostValue(ChangeEventArgs args)
        {
            this.selectedHost = this.ClusterConfig.ClusterDefinition.Hosts.Where(
                h => h.Name == args.Value.ToString()).FirstOrDefault();
        }

        private void ChangeHost()
        {
            this.ClusterConfig.CurrentHost = this.selectedHost;
        }
    }
}
