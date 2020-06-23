using System.Threading.Tasks;
using DG.Core.Model.ClusterConfig;
using Microsoft.AspNetCore.Components;

namespace DG.HostApp.Components.ClusterConfigEditor
{
    public partial class HostsCard
    {
        private string displayMode = FormElementsDisplayMode.NoneValue;
        private string selectedHostName = string.Empty;

        [Parameter]
        public Hosts Hosts { get; set; }

        private Host SelectedHost { get; set; }

        private void AddHost()
        {
            this.Hosts.Add(
                new Host
                {
                    Name = "NewHost",
                });
        }

        private Task<bool> DeleteHost(Host host)
        {
            return Task.Run(() =>
                  this.Hosts.Remove(host));
        }

        private string SetVisible(string hostName)
        {
            if (hostName == this.selectedHostName)
            {
                this.displayMode = FormElementsDisplayMode.DisplayRowValue;
            }
            else
            {
                this.displayMode = FormElementsDisplayMode.NoneValue;
            }

            return this.displayMode;
        }

        private void SetHost(Host host)
        {
            this.SelectedHost = host;
            this.selectedHostName = this.SelectedHost.Name;
        }

        private void Refresh(Host host)
        {
            this.displayMode = FormElementsDisplayMode.NoneValue;
            this.SelectedHost = host;
            this.selectedHostName = string.Empty;
        }
    }
}
