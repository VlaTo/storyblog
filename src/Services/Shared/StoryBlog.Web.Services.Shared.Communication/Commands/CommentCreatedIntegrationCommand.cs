namespace StoryBlog.Web.Services.Shared.Communication.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CommentCreatedIntegrationCommand : IntegrationCommand
    {
        /// <summary>
        /// 
        /// </summary>
        public string StorySlug
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long CommentId
        {
            get;
            set;
        }
    }
}