using System;
using System.Threading;
using StoryBlog.Web.Services.Shared.Common.Synchronization.Infrastructure;

namespace StoryBlog.Web.Services.Shared.Common.Synchronization
{
    public interface IWriterLockProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        AwaitableDisposable<IDisposable> AcquireWriterLockAsync(CancellationToken ct);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        AwaitableDisposable<IDisposable> AcquireWriterLockAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        IDisposable AcquireWriterLock(CancellationToken ct);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDisposable AcquireWriterLock();
    }
}