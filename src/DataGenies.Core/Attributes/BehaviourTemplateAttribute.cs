using System;

namespace DataGenies.Core.Attributes
{
    public class BehaviourTemplateAttribute : System.Attribute
    {
        private readonly Type _behaviourType;

        public BehaviourTemplateAttribute(Type behaviourType)
        {
            _behaviourType = behaviourType;
        }
    }
}