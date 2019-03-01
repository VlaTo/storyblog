﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://storyblog.org/schemas/json/models/landing")]
    public sealed class LandingModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DataMember(Name = "title")]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "description")]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "hero")]
        public HeroStoryModel Hero
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "featured")]
        public IEnumerable<FeedStoryModel> Featured
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "feed")]
        public IEnumerable<FeedStoryModel> Feed
        {
            get;
            set;
        }

        public LandingModel()
        {
            Featured = Enumerable.Empty<FeedStoryModel>();
            Feed = Enumerable.Empty<FeedStoryModel>();
        }
    }
}