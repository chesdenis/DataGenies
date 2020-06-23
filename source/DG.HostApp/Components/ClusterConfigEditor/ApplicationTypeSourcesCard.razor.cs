using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Core.Model.ClusterConfig;
using Microsoft.AspNetCore.Components;

namespace DG.HostApp.Components.ClusterConfigEditor
{
    public partial class ApplicationTypeSourcesCard
    {
        private string displayMode = FormElementsDisplayMode.NoneValue;
        private string selectedTypeName = string.Empty;

        [Parameter]
        public ClusterConfig ClusterConfig { get; set; }

        private ApplicationTypeSource SelectedTypeSource { get; set; }

        private void AddTypeSource()
        {
            this.ClusterConfig.ClusterDefinition.ApplicationTypesSources.Add(
                new ApplicationTypeSource { });
        }

        private Task<bool> DeleteAppTypeSource(ApplicationTypeSource applicationTypeSource)
        {
            return Task.Run(() =>
                  this.ClusterConfig.ClusterDefinition.ApplicationTypesSources.Remove(applicationTypeSource));
        }

        private string SetVisible(string applicationTypeSourceName)
        {
            if (applicationTypeSourceName == this.selectedTypeName)
            {
                this.displayMode = FormElementsDisplayMode.DisplayRowValue;
            }
            else
            {
                this.displayMode = FormElementsDisplayMode.NoneValue;
            }

            return this.displayMode;
        }

        private void SetTypeSource(ApplicationTypeSource applicationTypeSource)
        {
            this.SelectedTypeSource = applicationTypeSource;
            this.selectedTypeName = this.SelectedTypeSource.Name;
        }

        private void Refresh(ApplicationTypeSource applicationTypeSource)
        {
            this.displayMode = FormElementsDisplayMode.NoneValue;
            this.SelectedTypeSource = applicationTypeSource;
            this.selectedTypeName = string.Empty;
        }
    }
}
