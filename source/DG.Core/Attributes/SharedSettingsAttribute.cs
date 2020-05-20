using System;

namespace DG.Core.Attributes
{
    public class SharedSettingsAttribute : Attribute
    {
        public SharedSettingsAttribute(string pathToSharedSettings)
        {
            this.PathToSharedSettings = pathToSharedSettings;
        }
        
        public string PathToSharedSettings { get; set; }
    }
}