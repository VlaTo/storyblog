using StoryBlog.Web.Services.Blog.Application.Infrastructure;

namespace StoryBlog.Web.Services.Blog.API.Integration.Commands
{
    public sealed class StoryDeletedIntegrationCommand : IntegrationCommand
    {
        public long StoryId
        {
            get;
            set;
        }
    }
}