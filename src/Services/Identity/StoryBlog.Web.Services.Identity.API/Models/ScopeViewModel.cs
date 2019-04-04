namespace StoryBlog.Web.Services.Identity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ScopeViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Emphasize
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Required
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Checked
        {
            get;
            set;
        }
    }
}