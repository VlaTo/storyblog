﻿using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Common
{
    [DataContract(Name = "meta")]
    public sealed class ResultMetaInformation
    {
        [DataMember(Name = "navigation")]
        public Navigation Navigation
        {
            get;
            set;
        }
    }
}