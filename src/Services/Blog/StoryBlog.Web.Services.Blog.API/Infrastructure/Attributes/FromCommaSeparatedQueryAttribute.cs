using System;
using Microsoft.AspNetCore.Mvc;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromCommaSeparatedQueryAttribute : FromQueryAttribute
    {
        public char Separator
        {
            get;
            set;
        }

        public FromCommaSeparatedQueryAttribute()
        {
            Separator = ',';
        }
    }
}