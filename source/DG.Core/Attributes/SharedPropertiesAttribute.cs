using System;

namespace DG.Core.Attributes
{
    public class SharedPropertiesAttribute : Attribute
    {
        public SharedPropertiesAttribute(string propertyPath)
        {
            this.PropertyPath = propertyPath;
        }
        
        public string PropertyPath { get; set; }
    }
}