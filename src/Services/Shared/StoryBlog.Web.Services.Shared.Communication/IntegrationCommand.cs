using System;

namespace StoryBlog.Web.Services.Shared.Communication
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class IntegrationCommand
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset Sent
        {
            get;
            set;
        }
    }
}