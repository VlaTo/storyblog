using StoryBlog.Web.Services.Shared.Common;

namespace StoryBlog.Web.Services.Blog.API.Integration.Commands
{
    public sealed class CommentCreatedIntegrationCommand : IntegrationCommand
    {
        public string StorySlug
        {
            get;
            set;
        }

        public long CommentId
        {
            get;
            set;
        }
    }
}