using System;

namespace StoryBlog.Web.Services.Blog.Domain.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public DomainException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public DomainException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
