using System;

namespace StoryBlog.Web.Blazor.OidcClient2.Results
{
    /// <summary>
    /// Base class for results.
    /// </summary>
    public abstract class Result
    {
        /// <summary>
        /// Gets a value indicating whether this instance is error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is error; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsError => false == String.IsNullOrWhiteSpace(Error);

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public virtual string Error
        {
            get;
            set;
        }
    }
}
