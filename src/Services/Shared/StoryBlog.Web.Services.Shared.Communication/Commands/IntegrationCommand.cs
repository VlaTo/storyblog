using System;

namespace StoryBlog.Web.Services.Shared.Communication.Commands
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