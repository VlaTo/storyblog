﻿namespace StoryBlog.Web.Blazor.Client.Store.Models.Data
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FeedStory : StoryBase
    {
        /// <summary>
        /// 
        /// </summary>
        public int CommentsCount
        {
            get;
            set;
        }
    }
}