using StoryBlog.Web.Services.Blog.Application.Stories.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StoryBlog.Web.Services.Blog.Application.Models;

namespace StoryBlog.Web.Services.Blog.Application.Landing.Models
{
    public struct Landing
    {
        /// <summary>
        /// 
        /// </summary>
        /*public string Title
        {
            get;
            set;
        }*/

        /// <summary>
        /// 
        /// </summary>
        /*public string Description
        {
            get;
            set;
        }*/

        /// <summary>
        /// 
        /// </summary>
        /*public HeroStory HeroStory
        {
            get;
            set;
        }*/

        /// <summary>
        /// 
        /// </summary>
        /*public IReadOnlyCollection<FeedStory> FeaturedStories
        {
            get;
            private set;
        }*/

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<FeedStory> Entities
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Exception> Exceptions
        {
            get;
            private set;
        }

        public IEnumerator<FeedStory> GetEnumerator() => Entities.GetEnumerator();

        public static Landing Create(IList<FeedStory> feed)
        {
            return new Landing
            {
                Entities = new ReadOnlyCollection<FeedStory>(feed),
            };
        }

        public static Landing Error(params Exception[] exceptions)
        {
            return new Landing
            {
                Exceptions = exceptions
            };
        }
    }
}