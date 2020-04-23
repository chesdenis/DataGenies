using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DG.Core.Extensions
{
    public static class TypesExtensions
    {
        public static IEnumerable<Type> FilterTypesByClassAttribute(this IEnumerable<Type> types, Type attributeType)
        {
            return types.Where(w => w.GetCustomAttributes(attributeType).Any());
        }

        public static IEnumerable<Type> FilterTypesByMethodAttribute(this IEnumerable<Type> types, Type attributeType)
        {
            return types.Where(w => w.GetMethods()
                .Any(ww => ww.GetCustomAttributes(attributeType).Any()));
        }

        public static bool HasClassAttribute(this Type type, Type attributeType)
        {
            return type.GetCustomAttributes(attributeType).Any();
        }

        public static bool HasMethodAttribute(this Type type, Type attributeType)
        {
            return type.GetMethods()
                .Any(x => x.GetCustomAttributes(attributeType).Any());
        }
    }
}