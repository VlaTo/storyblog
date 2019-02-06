using StoryBlog.Web.Services.Shared.Common;

namespace StoryBlog.Web.Services.Blog.API.Integration.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StoryUpdatedIntegrationCommand : IntegrationCommand
    {
        /// <summary>
        /// 
        /// </summary>
        public long StoryId
        {
            get;
            set;
        }
    }
}