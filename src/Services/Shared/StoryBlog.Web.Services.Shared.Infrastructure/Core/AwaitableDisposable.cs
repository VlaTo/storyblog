using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Shared.Infrastructure.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AwaitableDisposable<T>
        where T : IDisposable
    {
        private readonly Task<T> task;

        /// <summary>
        /// Initializes a new awaitable wrapper around the specified task.
        /// </summary>
        /// <param name="task">The underlying task to wrap.</param>
        public AwaitableDisposable(Task<T> task)
        {
            this.task = task;
        }

        /// <summary>
        /// Returns the underlying task.
        /// </summary>
        public Task<T> AsTask()
        {
            return task;
        }

        /// <summary>
        /// Infrastructure. Returns the task awaiter for the underlying task.
        /// </summary>
        public TaskAwaiter<T> GetAwaiter()
        {
            return task.GetAwaiter();
        }

        /// <summary>
        /// Implicit conversion to the underlying task.
        /// </summary>
        /// <param name="source">The awaitable wrapper.</param>
        public static implicit operator Task<T>(AwaitableDisposable<T> source)
        {
            return source.AsTask();
        }

        /// <summary>
        /// Infrastructure. Returns a configured task awaiter for the underlying task.
        /// </summary>
        /// <param name="continueOnCapturedContext">Whether to attempt to marshal the continuation back to the captured context.</param>
        public ConfiguredTaskAwaitable<T> ConfigureAwait(bool continueOnCapturedContext)
        {
            return task.ConfigureAwait(continueOnCapturedContext);
        }
    }
}