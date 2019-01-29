using System;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Shared.Common.Synchronization
{
    public interface IAsyncWaitQueue<T>
    {
        bool IsEmpty
        {
            get;
        }

        Task<T> EnqueueAsync();

        IDisposable Dequeue(T result = default(T));

        IDisposable DequeueAll(T result = default(T));

        IDisposable TryCancel(Task task);

        IDisposable CancelAll();
    }
}