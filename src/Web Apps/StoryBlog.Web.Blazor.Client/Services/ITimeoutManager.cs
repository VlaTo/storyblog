using System;

namespace StoryBlog.Web.Client.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITimeout : IDisposable
    {

    }

    /// <summary>
    /// Defines method that called when timeout elapsed.
    /// </summary>
    public delegate void TimeoutCallback();

    /// <summary>
    /// Manages 
    /// </summary>
    public interface ITimeoutManager
    {
        /// <summary>
        /// Creates new timeout with <paramref name="callback" /> and <paramref name="timeout" /> specified.
        /// </summary>
        /// <param name="callback">The <see cref="TimeoutCallback" /> action to invoke.</param>
        /// <param name="timeout">The <see cref="TimeSpan" /> to delay.</param>
        /// <returns>The instance of the <see cref="ITimeout" /> created.</returns>
        ITimeout CreateTimeout(TimeoutCallback callback, TimeSpan timeout);
    }
}