using System;

namespace DG.Core.Extensions
{
    public static class StringExtensions
    {
        public static T ParseEnum<T>(this string enumValue) => (T)Enum.Parse(typeof(T), enumValue);
    }
}