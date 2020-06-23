using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Core.Model.ClusterConfig;
using DG.Core.Model.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace DG.HostApp.Components.ClusterConfigEditor
{
    public partial class ClusterDefinitionGroup
    {
        private string previousActive;
        private string currentActive = "Hosts";
        private Dictionary<string, string[]> elementsSettings = new Dictionary<string, string[]>()
{
        { "Hosts", new string[2] { "show active", "true" } },
        { "Type sources", new string[2] { string.Empty, "false" } },
        { "Instances", new string[2] { string.Empty, "false" } },
};

        [Parameter]
        public ClusterConfig ClusterConfig { get; set; }

        private List<ApplicationDto> AvailableApplications { get; set; } = new List<ApplicationDto>();

        protected override async Task OnInitializedAsync()
        {
            this.AvailableApplications = await this.clusterConfigPageService.ScanAvailableApplications(this.currentHost);
        }

        private Task Expand(string elementName)
        {
            return Task.Run(() =>
            {
                {
                    this.previousActive = this.currentActive;
                    this.currentActive = elementName;
                    this.elementsSettings[this.previousActive][0] = string.Empty;
                    this.elementsSettings[this.previousActive][1] = "false";
                    this.elementsSettings[elementName][0] = "show active";
                    this.elementsSettings[elementName][1] = "true";
                }
            });
        }
    }
}
