using System;

namespace StoryBlog.Web.Services.Blog.Application.Infrastructure
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
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Sent
        {
            get;
        }

        protected IntegrationCommand(Guid id, DateTime sent)
        {
            Id = id;
            Sent = sent;
        }
    }
}