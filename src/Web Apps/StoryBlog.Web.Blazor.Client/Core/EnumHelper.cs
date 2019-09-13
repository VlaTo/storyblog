using StoryBlog.Web.Client.Components.Attributes;
using System;
using System.Reflection;

namespace StoryBlog.Web.Client.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
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