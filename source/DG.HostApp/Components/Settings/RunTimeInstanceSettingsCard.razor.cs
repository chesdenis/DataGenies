using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Core.Model.Dto;
using Microsoft.AspNetCore.Components;

namespace DG.HostApp.Components.Settings
{
    public partial class RunTimeInstanceSettingsCard
    {
        [Parameter]
        public List<PropertyDTO> Properties { get; set; }

        private string PropertyTypeNameWithMargin(int nestingLevel, string typeName)
        {
            var typeNameWithMargin = new string('_', nestingLevel * 2) + typeName;
            return typeNameWithMargin;
        }
    }
}
