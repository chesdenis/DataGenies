using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Core.Model.Dto;
using DG.HostApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace DG.HostApp.Pages
{
    public partial class AppInstanceSettings
    {
        private const string Delimiter = "_";

        [Parameter]
        public string InstanceKey { get; set; }

        private List<PropertyDTO> Properties { get; set; } = new List<PropertyDTO>();

        protected override async Task OnInitializedAsync()
        {
            var index = this.InstanceKey.IndexOf(Delimiter);
            var typeName = this.InstanceKey.Substring(0, index);
            var instanceName = this.InstanceKey.Substring(index + 1);
            var settings = await this.orchestratorPageService.ScanInMemoryApplicationInstanceSettings(typeName.ToString(), instanceName.ToString(), this.currentHost);
            this.Properties = settings;
        }
    }
}
