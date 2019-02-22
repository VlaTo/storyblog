namespace StoryBlog.Web.Services.Shared.Communication.Commands
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