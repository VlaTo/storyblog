using StoryBlog.Web.Services.Shared.Common;

namespace StoryBlog.Web.Services.Blog.API.Integration.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StoryDeletedIntegrationCommand : IntegrationCommand
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