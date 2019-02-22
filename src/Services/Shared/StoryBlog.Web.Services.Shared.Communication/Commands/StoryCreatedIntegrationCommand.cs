namespace StoryBlog.Web.Services.Shared.Communication.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StoryCreatedIntegrationCommand : IntegrationCommand
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
