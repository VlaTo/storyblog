using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;

namespace StoryBlog.Web.Services.Blog.API.Integration.Commands
{
    public sealed class NewStoryCreatedCommand : IntegrationCommand
    {
        public long StoryId
        {
            get;
            set;
        }

        public string Slug
        {
            get;
            set;
        }

        public NewStoryCreatedCommand(Guid id, DateTime sent)
            : base(id, sent)
        {
        }
    }
}
