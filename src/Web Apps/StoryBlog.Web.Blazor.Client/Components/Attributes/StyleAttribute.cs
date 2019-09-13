﻿using System;

namespace StoryBlog.Web.Client.Components.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class StyleAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string ClassName
        {
            get;
        }

        public StyleAttribute(string className)
        {
            ClassName = className;
        }
    }
}
