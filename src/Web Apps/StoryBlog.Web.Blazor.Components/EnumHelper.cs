using System;
using System.Reflection;
using StoryBlog.Web.Blazor.Components.Attributes;

namespace StoryBlog.Web.Blazor.Components
{
    internal static class EnumHelper
    {
        public static string GetClassName<TEnum>(TEnum value) 
            where TEnum : struct
        {
            var enumType = typeof(TEnum);

            if (false == Enum.IsDefined(enumType, value))
            {
                throw new ArgumentException("Not defined", nameof(value));
            }

            var name = Enum.GetName(enumType, value);
            var property = enumType.GetField(name, BindingFlags.Static | BindingFlags.Public);
            var attribute = property?.GetCustomAttribute<StyleAttribute>();
            return attribute?.ClassName ?? name;
        }
    }
}