﻿using System;
using System.ComponentModel;

namespace StoryBlog.Web.Blazor.Reactive.Concurrency
{
    /// <summary>
    /// (Infrastructure) Concurrency abstraction layer interface.
    /// </summary>
    /// <remarks>
    /// This type is used by the Rx infrastructure and not meant for public consumption or implementation.
    /// No guarantees are made about forward compatibility of the type's functionality and its usage.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IConcurrencyAbstraction
    {
        /// <summary>
        /// Queues a method for execution at the specified relative time.
        /// </summary>
        /// <param name="action">Method to execute.</param>
        /// <param name="state">State to pass to the method.</param>
        /// <param name="dueTime">Time to execute the method on.</param>
        /// <returns>Disposable object that can be used to stop the timer.</returns>
        IDisposable StartTimer(Action<object> action, object state, TimeSpan dueTime);

        /// <summary>
        /// Queues a method for periodic execution based on the specified period.
        /// </summary>
        /// <param name="action">Method to execute; should be safe for reentrancy.</param>
        /// <param name="period">Period for running the method periodically.</param>
        /// <returns>Disposable object that can be used to stop the timer.</returns>
        IDisposable StartPeriodicTimer(Action action, TimeSpan period);

        /// <summary>
        /// Queues a method for execution.
        /// </summary>
        /// <param name="action">Method to execute.</param>
        /// <param name="state">State to pass to the method.</param>
        /// <returns>Disposable object that can be used to cancel the queued method.</returns>
        IDisposable QueueUserWorkItem(Action<object> action, object state);

        /// <summary>
        /// Blocking sleep operation.
        /// </summary>
        /// <param name="timeout">Time to sleep.</param>
        void Sleep(TimeSpan timeout);

        /// <summary>
        /// Starts a new stopwatch object.
        /// </summary>
        /// <returns>New stopwatch object; started at the time of the request.</returns>
        IStopwatch StartStopwatch();

        /// <summary>
        /// Gets whether long-running scheduling is supported.
        /// </summary>
        bool SupportsLongRunning { get; }

        /// <summary>
        /// Starts a new long-running thread.
        /// </summary>
        /// <param name="action">Method to execute.</param>
        /// <param name="state">State to pass to the method.</param>
        void StartThread(Action<object> action, object state);
    }
}