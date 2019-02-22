using System.Threading;
using StoryBlog.Web.Services.Shared.Infrastructure.Core;

namespace StoryBlog.Web.Services.Shared.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReaderWriterLock : IReaderLockProvider, IWriterLockProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        AwaitableDisposable<AsyncReaderWriterLock.UpgradeableReaderKey> AcquireUpgradeableReaderLockAsync(CancellationToken ct);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        AwaitableDisposable<AsyncReaderWriterLock.UpgradeableReaderKey> AcquireUpgradeableReaderLockAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        AsyncReaderWriterLock.UpgradeableReaderKey AcquireUpgradeableReaderLock(CancellationToken ct);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        AsyncReaderWriterLock.UpgradeableReaderKey AcquireUpgradeableReaderLock();
    }
}