using System;
using System.Threading;
using StoryBlog.Web.Services.Shared.Common.Synchronization.Infrastructure;

namespace StoryBlog.Web.Services.Shared.Common.Synchronization
{
    public interface IReaderLockProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        AwaitableDisposable<IDisposable> AcquireReaderLockAsync(CancellationToken ct);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        AwaitableDisposable<IDisposable> AcquireReaderLockAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        IDisposable AcquireReaderLock(CancellationToken ct);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDisposable AcquireReaderLock();
    }
}