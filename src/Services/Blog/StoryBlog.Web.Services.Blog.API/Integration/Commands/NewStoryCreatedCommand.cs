using StoryBlog.Web.Services.Blog.Application.Infrastructure;

namespace StoryBlog.Web.Services.Blog.API.Integration.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NewStoryCreatedCommand : IntegrationCommand
    {
        /// <summary>
        /// 
        /// </summary>
        public long StoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Slug
        {
            get;
            set;
        }
    }
}
