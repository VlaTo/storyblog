using StoryBlog.Web.Services.Blog.Application.Infrastructure;

namespace StoryBlog.Web.Services.Blog.API.Integration.Commands
{
    public sealed class StoryUpdatedIntegrationCommand : IntegrationCommand
    {
        public long StoryId
        {
            get;
            set;
        }
    }
}