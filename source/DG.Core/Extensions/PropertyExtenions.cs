namespace DG.Core.Extensions
{
    using System.Collections.Generic;
    using System.Reflection;
    using DG.Core.Model.Dto;

    public static class PropertyExtenions
    {
        public static List<PropertyDTO> GetPropertyDTOsToList(
            this object objectToGetInfoFrom,
            PropertyInfo propertyOfObject,
            List<PropertyDTO> settingsDTOs,
            string parentkey = "",
            int nestingLevel = 0)
        {
            var dto = new PropertyDTO();
            var propertyType = propertyOfObject.PropertyType;
            dto.PropertyTypeName = propertyType.Name;
            dto.PropertyName = propertyOfObject.Name;
            dto.NestingLevel = nestingLevel;
            dto.ParentKey = parentkey;
            settingsDTOs.Add(dto);

            if (propertyType.IsPrimitive || propertyType.Name == "String")
            {
                dto.ProperyValue = objectToGetInfoFrom != null ? propertyOfObject.GetValue(objectToGetInfoFrom) : string.Empty;
            }
            else
            {
                nestingLevel++;
                objectToGetInfoFrom = propertyOfObject.GetValue(objectToGetInfoFrom);
                var objectProperies = propertyType.GetProperties();
                foreach (var prop in objectProperies)
                {
                    objectToGetInfoFrom.GetPropertyDTOsToList(prop, settingsDTOs, propertyOfObject.PropertyType.Name + "/" + propertyOfObject.Name, nestingLevel);
                }
            }

            return settingsDTOs;
        }
    }
}
