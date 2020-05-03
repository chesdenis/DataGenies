namespace DG.Core.Attributes
{
    using System;

    public class PropertyAttribute : Attribute
    {
        public string Name { get; set; }

        public PropertyAttribute(string name)
        {
            this.Name = name;
        }
    }
}
