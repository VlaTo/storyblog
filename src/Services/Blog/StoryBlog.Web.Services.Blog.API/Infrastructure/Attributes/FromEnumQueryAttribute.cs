using System;
using Microsoft.AspNetCore.Mvc;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromEnumQueryAttribute : FromQueryAttribute, IEnumValuesQuery
    {
        public Type EnumType
        {
            get;
        }

        public FromEnumQueryAttribute(Type enumType)
        {
            EnumType = enumType;
        }
    }
}