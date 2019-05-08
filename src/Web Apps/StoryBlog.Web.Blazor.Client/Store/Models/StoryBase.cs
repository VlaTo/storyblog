﻿using System;

namespace StoryBlog.Web.Blazor.Client.Store.Models
{
    public abstract class StoryBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string Slug
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Author Author
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset Published
        {
            get;
            set;
        }

        protected StoryBase()
        {
        }
    }
}