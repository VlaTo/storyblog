using System.Collections.Generic;
using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Models
{
    public sealed class StoryRequestResult : RequestResult, IRequestResult<Story>
    {
        public Story Entity
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<Author> Authors
        {
            get;
        }


        public StoryRequestResult(Story entity, IReadOnlyCollection<Author> authors)
            : base(true, false)
        {
            Entity = entity;
        }
    }
}