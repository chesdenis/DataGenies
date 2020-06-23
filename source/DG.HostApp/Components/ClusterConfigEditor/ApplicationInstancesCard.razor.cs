using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Core.Model.ClusterConfig;

namespace DG.HostApp.Components.ClusterConfigEditor
{
    public partial class ApplicationInstancesCard
    {
       
        private string displayMode = FormElementsDisplayMode.NoneValue;
        private string selectedInstanceName = string.Empty;
        private string selectedInstanceType = string.Empty;
        private string newPlacement;

        [Parameter]
        public ClusterConfig ClusterConfig { get; set; }
        private ApplicationInstance SelectedInstance { get; set; }

        private void AddApplicationInstance()
        {
            this.ClusterConfig.ClusterDefinition.ApplicationInstances.Add(
                new ApplicationInstance
                {
                    Name = "NewAppication",
                    PlacementPolicies = new List<string>() { }
                });
        }

        private Task<bool> DeleteAppInstance(ApplicationInstance appInstance)
        {
            return Task.Run(() =>
                  this.ClusterConfig.ClusterDefinition.ApplicationInstances.Remove(appInstance)
                  );
        }

        private string SetVisible(string appInstanceName, string appType)
        {
            if (appInstanceName == selectedInstanceName && appType == selectedInstanceType)
            {
                displayMode = FormElementsDisplayMode.DisplayRowValue;
            }
            else displayMode = FormElementsDisplayMode.NoneValue;
            return displayMode;

        }

        private void SetAppInstance(ApplicationInstance instance)
        {
            this.SelectedInstance = instance;
            this.selectedInstanceName = this.SelectedInstance.Name;
            this.selectedInstanceType = this.SelectedInstance.Type;
        }

        private void Refresh(ApplicationInstance instance)
        {
            this.displayMode = FormElementsDisplayMode.NoneValue;
            this.SelectedInstance = instance;
            this.selectedInstanceName = string.Empty;
            this.selectedInstanceType = string.Empty;
        }

        private string GenerateLink(ApplicationInstance instance)
        {
            return $"instance-settings/{instance.Type + "_" + instance.Name}";
        }
    }
}
